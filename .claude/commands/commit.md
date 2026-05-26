---
description: Create a git commit from staged changes using the Semantic Commit Messages standard
---

Create a git commit for the **currently staged** changes only, following the
Semantic Commit Messages standard
(https://gist.github.com/joshbuchea/6f47e86d2510bce28f8e7f42ae84c716).

## Format

`<type>(<scope>): <subject>`

The `(<scope>)` is optional. Example: `feat: add hat wobble`,
`fix(auth): reject expired refresh tokens`.

## Allowed types

- **feat** — new user-facing feature
- **fix** — user-facing bug fix
- **docs** — documentation only
- **style** — formatting / whitespace, no production code change
- **refactor** — production code refactor with no behavior change
- **test** — add or refactor tests, no production code change
- **chore** — tooling / config / non-production code
- **build** — build system or external dependency changes
- **ci** — CI configuration only
- **perf** — performance improvement
- **revert** — revert a previous commit

When the diff mixes categories, pick the dominant one by user-visible impact
in this order: `feat > fix > perf > refactor > style > test > docs > build > ci > chore`.

## Rules

1. Subject is **imperative present tense** ("add", not "added" / "adds").
2. Subject lowercase after the colon; type lowercase; no trailing period.
3. Subject under 72 characters.
4. **English only** — never Hebrew (per project CLAUDE.md).
5. **No emojis** anywhere in the message (per project CLAUDE.md).
6. Scope (optional) is the area touched: e.g. `auth`, `reports`, `hangfire`,
   `worker-form`, `frontend`, `backend`. Pick from filenames in the diff.
7. If the change is non-trivial, add a body (blank line, then wrapped at ~72
   chars) explaining the **why**, not the what.
8. Do **not** add a `Co-Authored-By` trailer or any mention of an AI assistant.
9. Do **not** run `git add`. Do **not** amend. Do **not** push.

## Steps

1. Run `git status` to see what is staged.
2. If nothing is staged, stop and tell the user there is nothing to commit.
3. Run `git diff --cached` to see the staged change.
4. Run `git log -5 --pretty=format:"%s"` to sanity-check house style.
5. Pick the single best `type` and optional `scope` from the diff.
6. Write the subject (under 72 chars) describing the change.
7. If the change is non-trivial, draft a short body explaining motivation.
8. Commit using a HEREDOC for correct multi-line formatting:

```bash
git commit -m "$(cat <<'EOF'
<type>(<scope>): <subject>

<optional body>
EOF
)"
```

9. Run `git status` to confirm the commit landed.

$ARGUMENTS
