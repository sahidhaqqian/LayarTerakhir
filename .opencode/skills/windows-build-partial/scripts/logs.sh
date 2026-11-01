#!/usr/bin/env bash
# Show logs of a partial Windows build run. Delegates to the shared log store.
set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
exec "$SCRIPT_DIR/../../windows-build-r2/scripts/logs.sh" "$@"
