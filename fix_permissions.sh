#!/usr/bin/env bash

# find repo root
git_root="$(git rev-parse --show-toplevel 2>&1)"
rc=$?

# check we found a repo
if [ ${rc} -eq 128 ] ; then
    # we didn't find a repo
    echo ${git_root}
    exit ${rc}
elif [ ${rc} -gt 0 ] ; then
    # don't know what wrong, abort
    echo Unexpected error: ${git_root}
    exit ${rc}
elif [ "${git_root}" = "" ] ; then
    # if you run `git rev-parse` inside the `.git` directory it doesn't seem to
    # cause an error, but doesn't give you the root of the repo either
    echo Could not find the repository root directory
    exit 1
else
    echo Setting permissions for ${git_root}
fi

cd "${git_root}"

echo
echo Directories:
find . \
    -type d \
    ! -path "." \
    ! -path "./.git" \
    ! -path "./.git/*" \
    -exec chmod --changes 0755 {} \;

echo
echo Executables:
find . \
    -type f \
    ! -path "./.git/*" \
    -path "./*.sh" \
    -exec chmod --changes 0755 {} \;


echo
echo Files:
find . \
    -type f \
    ! -path "./.git/*" \
    ! -path "./*.sh" \
    -exec chmod --changes 0644 {} \;
