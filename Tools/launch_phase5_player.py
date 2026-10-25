"""Launch only our Phase5 review players in a bounded service outside tmux.
Pass run_phase5_player.py arguments, or --crash-probes. Returns immediately.
"""
import datetime,json,subprocess,sys
from pathlib import Path
ROOT=Path(__file__).resolve().parents[1]
assert sys.argv[1:]
unit='lt-p5-native-'+datetime.datetime.now().strftime('%Y%m%d-%H%M%S')
if sys.argv[1:]==['--crash-probes']:command=[sys.executable,str(ROOT/'Tools/check_phase5_crash_probes.py')]
elif sys.argv[1:]==['--ending-suite']:command=[sys.executable,str(ROOT/'Tools/check_phase5_endings.py')]
else:command=[sys.executable,str(ROOT/'Tools/run_phase5_player.py'),*sys.argv[1:]]
args=['systemd-run','--user','--collect','--unit='+unit,'--property=MemoryMax=4608M','--property=MemorySwapMax=64M','--property=TasksMax=256','--property=CPUQuota=250%','--property=OOMPolicy=stop','--property=WorkingDirectory='+str(ROOT),*command]
subprocess.run(args,check=True)
print(json.dumps(dict(unit=unit,command=command,memoryLimitBytes=4608*1024**2)))
