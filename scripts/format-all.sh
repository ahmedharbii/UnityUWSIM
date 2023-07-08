#!/bin/bash

exclude_list=(
    "Packages"
)

# Get the list of C# files tracked by Git
FILES=$(git ls-files "*.cs")

# Exclude specified directories from the file list
for exclude in "${exclude_list[@]}"; do
    FILES=$(echo "$FILES" | grep -v --invert-match "^$exclude/")
done

# Set the Internal Field Separator to newline
IFS=$'\n'

# Iterate over each file and format it using ClangFormat
for file in $FILES; do
    echo "Formatting $file"
    clang-format-11 -i "$file"
done

echo "Formatting complete!"
