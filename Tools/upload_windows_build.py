"""Upload exactly the requested ZIP to the configured R2 destination, then read back/hash it.
No rclone configuration/credentials or signed URLs are printed or stored in the receipt.
"""
from pathlib import Path
from datetime import datetime,timezone
import hashlib,json,subprocess
ROOT=Path(__file__).resolve().parents[1];archive=ROOT/'Builds/LayarTerakhir-Windows-x86_64-Development.zip'
destination='r2:layarterakhir/Layarterakhir/';remote=destination+archive.name
assert archive.is_file()
expected=hashlib.file_digest(archive.open('rb'),'sha256').hexdigest()
assert archive.with_suffix('.zip.sha256').read_text().split()[0]==expected,'Local checksum mismatch'
started=datetime.now(timezone.utc).isoformat()
subprocess.run(['rclone','copy',str(archive),destination,'-P'],check=True)
process=subprocess.Popen(['rclone','cat',remote],stdout=subprocess.PIPE)
digest=hashlib.sha256();size=0
for block in iter(lambda:process.stdout.read(1024*1024),b''):digest.update(block);size+=len(block)
assert process.wait()==0,'Remote readback failed'
assert size==archive.stat().st_size and digest.hexdigest()==expected,'Remote object differs from local ZIP'
record=dict(startedUtc=started,verifiedUtc=datetime.now(timezone.utc).isoformat(),remote=remote,sizeBytes=size,sha256=expected,uploadSucceeded=True,verification='Full remote object readback SHA-256 equals local archive',credentialsExposed=False)
(ROOT/'Audit/r2-windows-build-upload.json').write_text(json.dumps(record,indent=2)+'\n')
print('R2_UPLOAD_VERIFIED',size,expected,flush=True)
