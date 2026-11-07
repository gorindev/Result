#!/usr/bin/env bash
# Bumps <Version> in the csproj according to the gitflow branch name.
#   hotfix/*            -> patch bump of the version on origin/main
#   release/X.Y.Z       -> sets exactly X.Y.Z
#   release/major       -> major bump of origin/main
#   release/<anything>  -> minor bump of origin/main
# Idempotent: does nothing if the branch version is already greater than main's.
set -euo pipefail

BRANCH="${1:?branch name required}"
CSPROJ="src/Gorin.Result/Gorin.Result.csproj"
CHANGELOG="CHANGELOG.md"

read_version() { grep -oPm1 '(?<=<Version>)[0-9]+\.[0-9]+\.[0-9]+(?=</Version>)'; }

BASE=$(git show origin/main:"$CSPROJ" | read_version)
CURRENT=$(read_version < "$CSPROJ")
IFS=. read -r MA MI PA <<< "$BASE"

case "$BRANCH" in
  hotfix/*)            NEW="$MA.$MI.$((PA + 1))" ;;
  release/major)       NEW="$((MA + 1)).0.0" ;;
  release/[0-9]*.[0-9]*.[0-9]*) NEW="${BRANCH#release/}" ;;
  release/*)           NEW="$MA.$((MI + 1)).0" ;;
  *) echo "Not a release/hotfix branch: $BRANCH"; exit 0 ;;
esac

# semver compare: is $1 > $2 ?
gt() { [ "$(printf '%s\n%s\n' "$1" "$2" | sort -V | tail -1)" = "$1" ] && [ "$1" != "$2" ]; }

gt "$NEW" "$BASE" || { echo "::error::Computed version $NEW must be greater than main ($BASE)"; exit 1; }

if [ "$CURRENT" = "$NEW" ]; then echo "Already at $NEW"; exit 0; fi
if gt "$CURRENT" "$BASE" && [[ "$BRANCH" != release/[0-9]* ]]; then
  echo "Branch already bumped to $CURRENT (main is $BASE); leaving as is"; exit 0
fi

sed -i "s#<Version>[^<]*</Version>#<Version>$NEW</Version>#" "$CSPROJ"
if ! grep -q "^## $NEW\$" "$CHANGELOG"; then
  sed -i "0,/^## /s//## $NEW\n\n- TODO: describe changes.\n\n## /" "$CHANGELOG"
fi
echo "Version: $BASE -> $NEW"
echo "version=$NEW" >> "${GITHUB_OUTPUT:-/dev/null}"
