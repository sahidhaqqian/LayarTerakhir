"""Derive Stone/Earth/Wood footsteps from the three authored tile recordings.

The source assets stay the single recorded base; each surface is a filtered,
transient-shaped variant so a step never reuses the same ceramic sample.
"""
import wave
from pathlib import Path

import numpy as np

ROOT = Path(__file__).resolve().parents[1]
SOURCE = ROOT / "Assets/_LayarTerakhir/assets/Audio/Prepared"
TARGET = ROOT / "Assets/Resources/PlayerPresentation/Steps"
RATE = 32000


def read(path):
    with wave.open(str(path), "rb") as handle:
        assert handle.getnchannels() == 1 and handle.getsampwidth() == 2
        rate = handle.getframerate()
        data = np.frombuffer(handle.readframes(handle.getnframes()), dtype="<i2")
    return rate, data.astype(np.float64) / 32768.0


def write(path, samples, rate):
    path.parent.mkdir(parents=True, exist_ok=True)
    pcm = np.clip(samples, -1, 1)
    with wave.open(str(path), "wb") as handle:
        handle.setnchannels(1)
        handle.setsampwidth(2)
        handle.setframerate(rate)
        handle.writeframes((pcm * 32767).astype("<i2").tobytes())


def lowpass(samples, alpha):
    result = np.empty_like(samples)
    value = 0.0
    for index, sample in enumerate(samples):
        value += alpha * (sample - value)
        result[index] = value
    return result


def highpass(samples, alpha=0.92):
    return samples - lowpass(samples, alpha)


def normalise(samples, peak=0.92):
    maximum = float(np.max(np.abs(samples))) or 1.0
    return samples * (peak / maximum)


def windowed(samples, length):
    result = np.zeros(length)
    result[: min(length, len(samples))] = samples[:length]
    return result


def stone(source, rng):
    base = windowed(source, int(RATE * 0.115))
    body = highpass(base, 0.90)
    body = np.tanh(body * 2.4) * 0.72
    click = rng.normal(0, 1, len(body)) * np.exp(-np.linspace(0, 34, len(body))) * 0.20
    splash = lowpass(rng.normal(0, 1, len(body)), 0.20) * np.exp(-np.linspace(0, 18, len(body))) * 0.22
    return normalise(body + click + splash)


def earth(source, rng):
    base = windowed(source, int(RATE * 0.13))
    body = lowpass(base, 0.10) * 1.35
    thump = np.sin(2 * np.pi * 68 * np.arange(len(body)) / RATE) * np.exp(-np.linspace(0, 26, len(body)))
    grit = lowpass(rng.normal(0, 1, len(body)), 0.012) * np.exp(-np.linspace(0, 12, len(body))) * 0.28
    return normalise(body + thump * 0.42 + grit)


def wood(source, rng):
    base = windowed(source, int(RATE * 0.175))
    low = lowpass(base, 0.42)
    band = lowpass(base, 0.055)
    body = (low - band) * 1.4
    times = np.arange(len(body)) / RATE
    creak = (
        np.sin(2 * np.pi * (185 + 34 * np.sin(times * 21)) * times)
        + 0.42 * np.sin(2 * np.pi * 305 * times)
    ) * np.exp(-times * 15)
    grit = rng.normal(0, 1, len(body)) * np.exp(-np.linspace(0, 30, len(body))) * 0.06
    return normalise(body + creak * 0.30 + grit)


def main():
    sources = [read(SOURCE / f"FootstepTile{index}.wav") for index in range(3)]
    rate = sources[0][0]
    assert all(item[0] == rate for item in sources)
    rng = np.random.default_rng(1998)
    for index, (_, samples) in enumerate(sources):
        write(TARGET / "Stone" / f"FootstepStone{index}.wav", stone(samples, rng), rate)
        write(TARGET / "Earth" / f"FootstepEarth{index}.wav", earth(samples, rng), rate)
        write(TARGET / "Wood" / f"FootstepWood{index}.wav", wood(samples, rng), rate)
    print(f"Wrote 9 surface clips under {TARGET.relative_to(ROOT)}")


if __name__ == "__main__":
    main()
