#!/usr/bin/env bash
set -euo pipefail

PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
UNITY_EDITOR="${UNITY_EDITOR:-/root/Unity/Hub/Editor/6000.3.26f1/Editor/Unity}"
if [ ! -x "$UNITY_EDITOR" ]; then
    echo "Installed Unity 6000.3.26f1 editor not found: $UNITY_EDITOR" >&2
    exit 2
fi

# URP can automatically serialize a pipeline asset upgrade during Editor Play Mode.
# Preserve the exact pipeline asset supplied by the caller, including on a failed test run.
AUDIT_URP_BACKUP="$(mktemp /tmp/LayarTerakhir-urp.XXXXXX)"
cp "$PROJECT_ROOT/Assets/Settings/PC_RPAsset.asset" "$AUDIT_URP_BACKUP"
restore_urp_asset() {
    cp "$AUDIT_URP_BACKUP" "$PROJECT_ROOT/Assets/Settings/PC_RPAsset.asset"
    rm -f "$AUDIT_URP_BACKUP"
}
trap restore_urp_asset EXIT

"$UNITY_EDITOR" -batchmode -nographics -quit -projectPath "$PROJECT_ROOT" \
    -executeMethod LayarTerakhir.Editor.PrologMilestoneValidator.RunValidation \
    -logFile "$PROJECT_ROOT/Audit/final-validation.log"

# These Editor-discovered tests include UnityTests that explicitly enter Play Mode.
# Do not add -quit: the test runner exits after the asynchronous suite finishes.
"$UNITY_EDITOR" -batchmode -nographics -projectPath "$PROJECT_ROOT" \
    -runTests -testPlatform EditMode -testFilter LayarTerakhir.Editor.Tests \
    -testResults "$PROJECT_ROOT/Audit/foundation-tests.xml" \
    -logFile "$PROJECT_ROOT/Audit/foundation-tests.log"
