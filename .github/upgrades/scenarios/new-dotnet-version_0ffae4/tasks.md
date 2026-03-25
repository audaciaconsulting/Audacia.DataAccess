# Audacia.DataAccess .NET 10.0 Upgrade Tasks

## Overview

This document tracks the execution of the Audacia.DataAccess solution upgrade from .NET 8.0 to .NET 10.0. All 8 projects will be upgraded simultaneously in a single atomic operation, followed by comprehensive testing and validation.

**Progress**: 0/3 tasks complete (0%) ![0%](https://progress-bar.xyz/0)

---

## Tasks

### [ ] TASK-001: Verify prerequisites
**References**: Plan §Implementation Timeline Phase 0

- [ ] (1) Verify .NET 10 SDK installed per Plan §Prerequisites
- [ ] (2) SDK version meets minimum requirements (**Verify**)

---

### [ ] TASK-002: Atomic framework and dependency upgrade
**References**: Plan §Implementation Timeline Phase 1, Plan §Package Update Reference, Plan §Breaking Changes Catalog

- [ ] (1) Update `<TargetFramework>` from net8.0 to net10.0 in all 8 project files listed in Plan §Project-by-Project Plans
- [ ] (2) All project files updated to net10.0 (**Verify**)
- [ ] (3) Update Entity Framework Core packages per Plan §Package Update Reference (Microsoft.EntityFrameworkCore, Microsoft.EntityFrameworkCore.Relational, Microsoft.EntityFrameworkCore.SqlServer, Microsoft.EntityFrameworkCore.InMemory from 8.0.11 to 10.0.5 across affected projects)
- [ ] (4) All EF Core packages updated to 10.0.5 (**Verify**)
- [ ] (5) Remove System.Net.Http and System.Text.RegularExpressions package references from Triggers and Auditing projects per Plan §Package Update Reference
- [ ] (6) System.* packages removed from both projects (**Verify**)
- [ ] (7) Restore all dependencies
- [ ] (8) All dependencies restored successfully (**Verify**)
- [ ] (9) Build solution and fix all compilation errors per Plan §Breaking Changes Catalog (expected: none detected, but address any that arise)
- [ ] (10) Solution builds with 0 errors (**Verify**)
- [ ] (11) Commit changes with message: "feat: Upgrade solution to .NET 10.0 #204001"

---

### [ ] TASK-003: Run full test suite and validate upgrade
**References**: Plan §Testing & Validation Strategy, Plan §Project-by-Project Plans (Tests)

- [ ] (1) Run tests in Audacia.DataAccess.Tests project
- [ ] (2) Fix any test failures (reference Plan §Breaking Changes Catalog if needed, though none detected in assessment)
- [ ] (3) Re-run tests after fixes
- [ ] (4) All tests pass with 0 failures (**Verify**)
- [ ] (5) Commit test fixes with message: "feat: Complete testing and validation #204001"

---