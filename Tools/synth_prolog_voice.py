"""Compatibility entrypoint. eSpeak generation was retired after player feedback.
Run with the voice-authoring virtualenv containing VoxCPM2 / CPU PyTorch / soundfile.
"""
from pathlib import Path
import runpy
runpy.run_path(str(Path(__file__).with_name('neural_prolog_voice.py')),run_name='__main__')
