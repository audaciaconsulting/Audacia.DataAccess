# .NET 10.0 Upgrade Plan

## Table of Contents

- [Executive Summary](#executive-summary)
- [Migration Strategy](#migration-strategy)
- [Detailed Dependency Analysis](#detailed-dependency-analysis)
- [Project-by-Project Plans](#project-by-project-plans)
- [Package Update Reference](#package-update-reference)
- [Breaking Changes Catalog](#breaking-changes-catalog)
- [Risk Management](#risk-management)
- [Testing & Validation Strategy](#testing--validation-strategy)
- [Complexity & Effort Assessment](#complexity--effort-assessment)
- [Source Control Strategy](#source-control-strategy)
- [Success Criteria](#success-criteria)

---

## Executive Summary

### Scenario Description

Upgrade the **Audacia.DataAccess** solution from **.NET 8.0** to **.NET 10.0 (LTS)**. The solution consists of 8 projects: 7 class libraries providing data access abstractions and Entity Framework Core utilities, plus 1 test project.

### Scope

**Projects Affected:** 8 projects
- 6 core library projects (Commands, Model, EntityFrameworkCore, SqlServer, Triggers, Auditing)
- 1 base library (DataAccess)
- 1 test project

**Current State:** All projects targeting net8.0
**Target State:** All projects targeting net10.0

### Selected Strategy

**All-At-Once Strategy** - All projects upgraded simultaneously in single coordinated operation.

**Rationale:**
- Small solution with 8 projects (well within All-At-Once threshold)
- All projects currently on .NET 8.0 (homogeneous baseline)
- Clear, simple dependency structure (3 levels deep)
- All projects marked Low difficulty
- All packages have known .NET 10-compatible versions
- No security vulnerabilities or blocking issues
- Good candidate for atomic upgrade

### Complexity Assessment

**Discovered Metrics:**
- Total Projects: 8
- Total NuGet Packages: 14 (4 require upgrade, 2 require removal)
- Lines of Code: 6,510
- Dependency Depth: 3 levels
- High-Risk Projects: 0
- Security Vulnerabilities: 0
- API Breaking Changes Detected: 0

**Classification:** Simple Solution

### Critical Issues

**Good News:** No critical blockers identified
- ✅ No security vulnerabilities
- ✅ No API breaking changes detected
- ✅ All packages have .NET 10-compatible versions
- ✅ All projects are SDK-style (no conversion needed)
- ✅ Clear dependency structure with no circular dependencies

**Required Actions:**
- Update all project TargetFramework properties from net8.0 to net10.0
- Upgrade 4 Entity Framework Core packages from 8.0.11 to 10.0.5
- Remove 2 packages now included in framework (System.Net.Http, System.Text.RegularExpressions)

### Recommended Approach

**All-At-Once Migration:** Update all 8 projects atomically in a single operation. This approach is optimal for this solution due to its small size, simple structure, and lack of complexity indicators.

**Execution Flow:**
1. Update all project files simultaneously
2. Update all package references in one operation
3. Build entire solution and address any compilation errors
4. Run all tests to validate functionality

### Iteration Strategy

Using **fast batch approach** for plan generation:
- Phase 1: Discovery & Classification (3 iterations) ✓
- Phase 2: Foundation (3 iterations)
- Phase 3: Detail Generation (2 iterations for all projects)

**Estimated Total:** 8 iterations

---

## Migration Strategy

### Approach Selection: All-At-Once Strategy

**Selected Approach:** Upgrade all 8 projects simultaneously in a single atomic operation.

### Justification

This solution is an **ideal candidate for All-At-Once migration**:

✅ **Small Solution Size**
- 8 projects (well below 30-project threshold)
- 6,510 total lines of code
- Manageable testing surface

✅ **Homogeneous Baseline**
- All projects currently on .NET 8.0
- All projects are SDK-style (no conversion needed)
- Consistent project structure

✅ **Simple Dependency Structure**
- Clean 3-tier dependency graph
- No circular dependencies
- Clear leaf-to-root ordering

✅ **Low Complexity**
- All projects marked "Low" difficulty
- No high-risk projects
- No security vulnerabilities

✅ **Package Compatibility**
- All 4 packages requiring updates have clear .NET 10 versions
- 2 packages to remove (now in framework) - straightforward
- No compatibility concerns flagged

✅ **Risk Profile**
- No API breaking changes detected
- No major architectural changes required
- Comprehensive test coverage available

### All-At-Once Strategy Characteristics

**Advantages for This Migration:**
- **Fastest Completion:** Single operation vs. multiple phases
- **No Multi-Targeting:** Avoid complexity of supporting multiple framework versions
- **Unified Testing:** Test entire solution at once after upgrade
- **Simple Coordination:** All developers work with same target framework
- **Clean State:** No intermediate partially-upgraded states

**Trade-offs Accepted:**
- **Higher Initial Risk:** All projects change at once (mitigated by low complexity)
- **Coordinated Testing:** Entire solution must be validated together (feasible with 8 projects)
- **Atomic Deployment:** All projects deploy with new framework (acceptable for library solution)

### Dependency-Based Ordering Rationale

While updates happen atomically, the **build system naturally respects dependency order**:

**Tier 1 (Foundation):**
- `Audacia.DataAccess.Model` - No dependencies, compiles first
- `Audacia.DataAccess` - No dependencies, compiles first

**Tier 2 (Core Implementations):**
- Projects depending on Tier 1 compile after their dependencies are built
- `Commands`, `EntityFrameworkCore`, `SqlServer`, `Triggers` build once foundation is ready

**Tier 3 (Advanced Features):**
- `Auditing` builds after `Model` and `Triggers` are available

**Tier 4 (Tests):**
- `Tests` project builds after all implementation projects are complete

This ordering is **automatic** - no manual intervention required. The All-At-Once strategy means we update all project files simultaneously, then let the build system handle compilation order.

### Execution vs. Sequential Approach

**All-At-Once Execution:**
1. Update all 8 project files (.csproj) to net10.0
2. Update all package references across all projects
3. Restore dependencies (`dotnet restore`)
4. Build entire solution (`dotnet build`)
5. Fix any compilation errors found (expected to be minimal/none)
6. Run all tests (`dotnet test`)

**Total Operations:** One atomic upgrade operation + one test validation

**Sequential Approach (NOT USED):**
Would require 8 separate project migrations, 8 separate test validations, maintaining multi-targeted dependencies throughout. Not justified for this simple solution.

### Parallel vs. Sequential Execution Decision

**Chosen:** Sequential execution of the single atomic operation

**Rationale:**
- Only one "batch" to execute (all 8 projects)
- Build system handles internal parallelization
- No need for developer-level parallel workflows
- Simplifies coordination and validation

### Risk Management Alignment

The All-At-Once approach is **low risk** for this solution because:
- Small codebase size limits blast radius
- No breaking changes detected reduces uncertainty
- Comprehensive test suite provides validation safety net
- Simple rollback: revert branch to previous commit
- All projects owned by same team (no cross-team coordination needed)

### Success Criteria for All-At-Once

The migration succeeds when **all of these are true simultaneously**:
- ✅ All 8 projects build without errors
- ✅ All 8 projects build without warnings
- ✅ All tests pass (no regressions)
- ✅ No package dependency conflicts
- ✅ No security vulnerabilities remain

There are **no intermediate success states** - the solution is either fully migrated or not migrated.

---

## Detailed Dependency Analysis

### Dependency Graph Summary

The solution has a clean 3-tier dependency structure with clear separation:

**Tier 1: Foundation (Leaf Nodes - 2 projects)**
- `Audacia.DataAccess.Model.csproj` - No project dependencies
- `Audacia.DataAccess.csproj` - No project dependencies

**Tier 2: Core Implementations (4 projects)**
- `Audacia.DataAccess.Commands.csproj` → depends on DataAccess
- `Audacia.DataAccess.EntityFrameworkCore.csproj` → depends on DataAccess
- `Audacia.DataAccess.EntityFrameworkCore.SqlServer.csproj` → depends on DataAccess
- `Audacia.DataAccess.EntityFrameworkCore.Triggers.csproj` → depends on Model

**Tier 3: Advanced Features (1 project)**
- `Audacia.DataAccess.EntityFrameworkCore.Auditing.csproj` → depends on Model and Triggers

**Tier 4: Tests (1 project)**
- `Audacia.DataAccess.Tests.csproj` → depends on DataAccess, EntityFrameworkCore, and SqlServer

### Project Groupings for All-At-Once Migration

Since this is an **All-At-Once strategy**, all projects will be upgraded **simultaneously in a single atomic operation**. The dependency tiers above are shown for structural understanding, not for sequential migration phases.

**Single Migration Group: All 8 Projects**
- All project files updated to net10.0 at once
- All package references updated at once
- Solution built as a whole after updates
- All tests run together after successful build

### Critical Path Identification

For the All-At-Once approach, the critical path is the **atomic upgrade operation itself**:

1. **Preparation:** Ensure all prerequisites (SDK, tooling) are in place
2. **Atomic Update:** Update all 8 project files and package references
3. **Build Validation:** Build entire solution to identify any issues
4. **Test Validation:** Run all tests to confirm functionality

**No intermediate validation checkpoints** between individual projects - the solution moves as one unit from net8.0 to net10.0.

### Circular Dependencies

**Status:** ✅ None detected

The dependency graph is a clean directed acyclic graph (DAG) with no circular references.

### Dependency Considerations for All-At-Once

**Why All-At-Once Works Here:**
- Foundation projects (Model, DataAccess) have minimal dependencies
- All projects can compile once their dependencies are at net10.0
- No cross-tier dependencies that would complicate atomic updates
- Test project sits at top of tree - natural final validation point

**Execution Order Within Atomic Operation:**
While all updates happen in one batch, the internal processing order respects dependencies:
1. Update foundation projects first (Model, DataAccess)
2. Update dependent projects (Commands, EF Core utilities)
3. Update higher-tier projects (Auditing)
4. Update test project last

This internal ordering is handled automatically by the build system - no manual sequencing required.

---

## Project-by-Project Plans

### Project: Audacia.DataAccess.Model

**Current State:** net8.0, ClassLibrary, SDK-style, 99 LOC, 0 dependencies, 2 dependants
**Target State:** net10.0
**Risk Level:** Low

#### Migration Steps

1. **Prerequisites**
   - None (leaf node with no dependencies)

2. **Technology/Framework Update**
   - Update `<TargetFramework>` in `src\Audacia.DataAccess.Model\Audacia.DataAccess.Model.csproj`
   - Change from: `net8.0`
   - Change to: `net10.0`

3. **Package/Module/Dependency Updates**
   - No package updates required (uses Audacia.CodeAnalysis 1.5.1, which is compatible)

4. **Expected Breaking Changes**
   - None detected by assessment
   - No API incompatibilities found

5. **Code Modifications**
   - No code changes expected
   - Project contains only model classes (5 files)
   - Simple data structures unlikely affected by framework changes

6. **Testing Strategy**
   - **Validation Method:** Compile verification
   - Project builds without errors
   - No direct tests (validated via Auditing and Triggers projects)
   - Test coverage comes from dependant projects

7. **Validation Checklist**
   - [ ] Project file updated to net10.0
   - [ ] Project builds without errors
   - [ ] Project builds without warnings
   - [ ] No NuGet restore conflicts
   - [ ] Dependant projects (Triggers, Auditing) still reference correctly

---

### Project: Audacia.DataAccess

**Current State:** net8.0, ClassLibrary, SDK-style, 2,454 LOC, 0 dependencies, 4 dependants
**Target State:** net10.0
**Risk Level:** Low

#### Migration Steps

1. **Prerequisites**
   - None (leaf node with no dependencies)
   - Note: Largest codebase (2,454 LOC, 41 files) - foundation for 4 other projects

2. **Technology/Framework Update**
   - Update `<TargetFramework>` in `src\Audacia.DataAccess\Audacia.DataAccess.csproj`
   - Change from: `net8.0`
   - Change to: `net10.0`

3. **Package/Module/Dependency Updates**
   - No package updates required
   - Uses Audacia.Core 1.1.2 (compatible)
   - Uses Audacia.CodeAnalysis 1.5.1 (compatible)

4. **Expected Breaking Changes**
   - None detected by assessment
   - 908 APIs analyzed, all compatible
   - No binary, source, or behavioral incompatibilities

5. **Code Modifications**
   - No code changes expected
   - Largest project but all APIs compatible
   - Review areas if compiler errors arise:
     - Repository abstractions
     - Query specification patterns
     - Unit of work implementations

6. **Testing Strategy**
   - **Direct Tests:** Validated via `Audacia.DataAccess.Tests`
   - **Indirect Tests:** Exercised through 4 dependant projects
   - **Validation:** Full test suite execution

7. **Validation Checklist**
   - [ ] Project file updated to net10.0
   - [ ] Project builds without errors
   - [ ] Project builds without warnings
   - [ ] All 4 dependant projects compile correctly
   - [ ] No NuGet restore conflicts
   - [ ] Test project passes all tests

---

### Project: Audacia.DataAccess.Commands

**Current State:** net8.0, ClassLibrary, SDK-style, 82 LOC, 1 dependency, 0 dependants
**Target State:** net10.0
**Risk Level:** Low

#### Migration Steps

1. **Prerequisites**
   - `Audacia.DataAccess` must be updated to net10.0 (part of same atomic operation)

2. **Technology/Framework Update**
   - Update `<TargetFramework>` in `src\Audacia.DataAccess.Commands\Audacia.DataAccess.Commands.csproj`
   - Change from: `net8.0`
   - Change to: `net10.0`

3. **Package/Module/Dependency Updates**
   - No package updates required
   - Uses Audacia.Commands 1.1.1 (compatible)
   - Uses Audacia.Core 1.1.2 (compatible)
   - Uses Audacia.CodeAnalysis 1.5.1 (compatible)

4. **Expected Breaking Changes**
   - None detected
   - 46 APIs analyzed, all compatible
   - Smallest codebase (82 LOC, 1 file)

5. **Code Modifications**
   - No code changes expected
   - Simple command pattern implementations
   - Minimal surface area for issues

6. **Testing Strategy**
   - **Validation Method:** Compile verification
   - No direct tests visible
   - Validated through consumer usage

7. **Validation Checklist**
   - [ ] Project file updated to net10.0
   - [ ] Dependency on DataAccess.csproj resolves correctly
   - [ ] Project builds without errors
   - [ ] Project builds without warnings
   - [ ] No NuGet restore conflicts

---

### Project: Audacia.DataAccess.EntityFrameworkCore

**Current State:** net8.0, ClassLibrary, SDK-style, 307 LOC, 1 dependency, 1 dependant
**Target State:** net10.0
**Risk Level:** Low

#### Migration Steps

1. **Prerequisites**
   - `Audacia.DataAccess` must be updated to net10.0 (part of same atomic operation)

2. **Technology/Framework Update**
   - Update `<TargetFramework>` in `src\Audacia.DataAccess.EntityFrameworkCore\Audacia.DataAccess.EntityFrameworkCore.csproj`
   - Change from: `net8.0`
   - Change to: `net10.0`

3. **Package/Module/Dependency Updates**

   | Package | Current Version | Target Version | Reason |
   |---------|----------------|----------------|--------|
   | Microsoft.EntityFrameworkCore | 8.0.11 | 10.0.5 | Framework compatibility |

   **Other Packages (No Update):**
   - Audacia.Core 1.1.2 (compatible)
   - Audacia.CodeAnalysis 1.5.1 (compatible)

4. **Expected Breaking Changes**
   - **Assessment Status:** No breaking changes detected (200 APIs analyzed, all compatible)
   - **Potential Areas to Monitor:**
     - EF Core 9 and 10 introduced new features but minimal breaking changes
     - Check for deprecated method usage warnings during build
     - Review EF Core 10 release notes: [https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-10.0/whatsnew](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-10.0/whatsnew)

5. **Code Modifications**
   - **Expected:** None (assessment found no incompatibilities)
   - **If Needed:** Address any compiler warnings about obsolete APIs
   - **Review Areas:**
     - DbContext extensions (5 files in project)
     - Query interceptors or conventions
     - Transaction handling

6. **Testing Strategy**
   - **Direct Tests:** Validated via `Audacia.DataAccess.Tests`
   - **Coverage:** Test project uses EF Core InMemory provider
   - **Validation:** Full test suite execution

7. **Validation Checklist**
   - [ ] Project file updated to net10.0
   - [ ] Microsoft.EntityFrameworkCore updated to 10.0.5
   - [ ] Dependency on DataAccess.csproj resolves correctly
   - [ ] Project builds without errors
   - [ ] Project builds without warnings
   - [ ] No NuGet package conflicts
   - [ ] Test project passes all tests
   - [ ] Dependant project (Tests) compiles correctly

---

### Project: Audacia.DataAccess.EntityFrameworkCore.SqlServer

**Current State:** net8.0, ClassLibrary, SDK-style, 687 LOC, 1 dependency, 1 dependant
**Target State:** net10.0
**Risk Level:** Low

#### Migration Steps

1. **Prerequisites**
   - `Audacia.DataAccess` must be updated to net10.0 (part of same atomic operation)

2. **Technology/Framework Update**
   - Update `<TargetFramework>` in `src\Audacia.DataAccess.EntityFrameworkCore.SqlServer\Audacia.DataAccess.EntityFrameworkCore.SqlServer.csproj`
   - Change from: `net8.0`
   - Change to: `net10.0`

3. **Package/Module/Dependency Updates**

   | Package | Current Version | Target Version | Reason |
   |---------|----------------|----------------|--------|
   | Microsoft.EntityFrameworkCore | 8.0.11 | 10.0.5 | Framework compatibility |
   | Microsoft.EntityFrameworkCore.SqlServer | 8.0.11 | 10.0.5 | SQL Server provider compatibility |

   **Other Packages (No Update):**
   - Audacia.Core 1.1.2 (compatible)
   - Audacia.CodeAnalysis 1.5.1 (compatible)

4. **Expected Breaking Changes**
   - **Assessment Status:** No breaking changes detected (422 APIs analyzed, all compatible)
   - **Potential Areas to Monitor:**
     - SQL Server provider behavior changes (consult EF Core 10 release notes)
     - Connection string handling
     - Migration generation (if migrations present)

5. **Code Modifications**
   - **Expected:** None (assessment found no incompatibilities)
   - **If Needed:** Review SQL Server-specific extensions
   - **Review Areas:**
     - DbContext configuration for SQL Server
     - SQL Server-specific query extensions (2 files)
     - Connection resiliency configuration

6. **Testing Strategy**
   - **Direct Tests:** Validated via `Audacia.DataAccess.Tests`
   - **Coverage:** Test project references this project directly
   - **Validation:** Full test suite execution

7. **Validation Checklist**
   - [ ] Project file updated to net10.0
   - [ ] Microsoft.EntityFrameworkCore updated to 10.0.5
   - [ ] Microsoft.EntityFrameworkCore.SqlServer updated to 10.0.5
   - [ ] Dependency on DataAccess.csproj resolves correctly
   - [ ] Project builds without errors
   - [ ] Project builds without warnings
   - [ ] No NuGet package conflicts
   - [ ] Test project passes all tests

---

### Project: Audacia.DataAccess.EntityFrameworkCore.Triggers

**Current State:** net8.0, ClassLibrary, SDK-style, 788 LOC, 1 dependency, 1 dependant
**Target State:** net10.0
**Risk Level:** Low

#### Migration Steps

1. **Prerequisites**
   - `Audacia.DataAccess.Model` must be updated to net10.0 (part of same atomic operation)

2. **Technology/Framework Update**
   - Update `<TargetFramework>` in `src\Audacia.DataAccess.EntityFrameworkCore.Triggers\Audacia.DataAccess.EntityFrameworkCore.Triggers.csproj`
   - Change from: `net8.0`
   - Change to: `net10.0`

3. **Package/Module/Dependency Updates**

   | Package | Current Version | Target Version | Reason |
   |---------|----------------|----------------|--------|
   | Microsoft.EntityFrameworkCore | 8.0.11 | 10.0.5 | Framework compatibility |

   **Packages to REMOVE (Now in Framework):**
   - `System.Net.Http` 4.3.4 - Functionality included in .NET 10 framework
   - `System.Text.RegularExpressions` 4.3.1 - Functionality included in .NET 10 framework

   **Other Packages (No Update):**
   - Audacia.Core 1.1.2 (compatible)
   - Audacia.CodeAnalysis 1.5.1 (compatible)
   - Audacia.CodeAnalysis.Analyzers.Helpers 1.1.2 (compatible)

4. **Expected Breaking Changes**
   - **Assessment Status:** No breaking changes detected (406 APIs analyzed, all compatible)
   - **Package Removals:** Safe - `System.Net.Http` and `System.Text.RegularExpressions` are now part of the .NET runtime
   - **Potential Areas to Monitor:**
     - EF Core trigger implementation patterns
     - Interceptor API changes (if used)

5. **Code Modifications**
   - **Expected:** None
   - **Package Removal Impact:** Transparent - framework provides same APIs
   - **Review Areas:**
     - Trigger registration logic (8 files)
     - Entity tracking interceptors
     - Save changes hooks

6. **Testing Strategy**
   - **Indirect Tests:** Exercised through `Audacia.DataAccess.EntityFrameworkCore.Auditing`
   - **Coverage:** Auditing project depends on Triggers
   - **Validation:** Compile verification + dependant project testing

7. **Validation Checklist**
   - [ ] Project file updated to net10.0
   - [ ] Microsoft.EntityFrameworkCore updated to 10.0.5
   - [ ] System.Net.Http package reference removed
   - [ ] System.Text.RegularExpressions package reference removed
   - [ ] Dependency on Model.csproj resolves correctly
   - [ ] Project builds without errors
   - [ ] Project builds without warnings
   - [ ] No NuGet package conflicts
   - [ ] Dependant project (Auditing) compiles correctly

---

### Project: Audacia.DataAccess.EntityFrameworkCore.Auditing

**Current State:** net8.0, ClassLibrary, SDK-style, 1,598 LOC, 2 dependencies, 0 dependants
**Target State:** net10.0
**Risk Level:** Low

#### Migration Steps

1. **Prerequisites**
   - `Audacia.DataAccess.Model` must be updated to net10.0 (part of same atomic operation)
   - `Audacia.DataAccess.EntityFrameworkCore.Triggers` must be updated to net10.0 (part of same atomic operation)

2. **Technology/Framework Update**
   - Update `<TargetFramework>` in `src\Audacia.DataAccess.EntityFrameworkCore.Auditing\Audacia.DataAccess.EntityFrameworkCore.Auditing.csproj`
   - Change from: `net8.0`
   - Change to: `net10.0`

3. **Package/Module/Dependency Updates**

   | Package | Current Version | Target Version | Reason |
   |---------|----------------|----------------|--------|
   | Microsoft.EntityFrameworkCore | 8.0.11 | 10.0.5 | Framework compatibility |
   | Microsoft.EntityFrameworkCore.Relational | 8.0.11 | 10.0.5 | Relational database support compatibility |

   **Packages to REMOVE (Now in Framework):**
   - `System.Net.Http` 4.3.4 - Functionality included in .NET 10 framework
   - `System.Text.RegularExpressions` 4.3.1 - Functionality included in .NET 10 framework

   **Other Packages (No Update):**
   - Audacia.Core 1.1.2 (compatible)
   - Audacia.CodeAnalysis 1.5.1 (compatible)

4. **Expected Breaking Changes**
   - **Assessment Status:** No breaking changes detected (896 APIs analyzed, all compatible)
   - **Package Removals:** Safe - functionality remains in framework
   - **Potential Areas to Monitor:**
     - EF Core auditing interceptor APIs
     - Change tracking behavior
     - Entity state management

5. **Code Modifications**
   - **Expected:** None (largest EF Core project at 1,598 LOC, but all APIs compatible)
   - **Package Removal Impact:** Transparent - framework provides same APIs
   - **Review Areas:**
     - Audit trail generation logic (19 files)
     - Entity change tracking
     - Temporal table support (if used)
     - User context tracking

6. **Testing Strategy**
   - **Validation Method:** Compile verification
   - No direct tests visible
   - Validated through integration with consuming applications
   - **Recommendation:** Consider adding unit tests for auditing logic

7. **Validation Checklist**
   - [ ] Project file updated to net10.0
   - [ ] Microsoft.EntityFrameworkCore updated to 10.0.5
   - [ ] Microsoft.EntityFrameworkCore.Relational updated to 10.0.5
   - [ ] System.Net.Http package reference removed
   - [ ] System.Text.RegularExpressions package reference removed
   - [ ] Dependencies on Model.csproj and Triggers.csproj resolve correctly
   - [ ] Project builds without errors
   - [ ] Project builds without warnings
   - [ ] No NuGet package conflicts

---

### Project: Audacia.DataAccess.Tests

**Current State:** net8.0, DotNetCoreApp, SDK-style, 495 LOC, 3 dependencies, 0 dependants
**Target State:** net10.0
**Risk Level:** Low

#### Migration Steps

1. **Prerequisites**
   - `Audacia.DataAccess` must be updated to net10.0 (part of same atomic operation)
   - `Audacia.DataAccess.EntityFrameworkCore` must be updated to net10.0 (part of same atomic operation)
   - `Audacia.DataAccess.EntityFrameworkCore.SqlServer` must be updated to net10.0 (part of same atomic operation)

2. **Technology/Framework Update**
   - Update `<TargetFramework>` in `tests\Audacia.DataAccess.Tests\Audacia.DataAccess.Tests.csproj`
   - Change from: `net8.0`
   - Change to: `net10.0`

3. **Package/Module/Dependency Updates**

   | Package | Current Version | Target Version | Reason |
   |---------|----------------|----------------|--------|
   | Microsoft.EntityFrameworkCore | 8.0.11 | 10.0.5 | Framework compatibility |
   | Microsoft.EntityFrameworkCore.InMemory | 8.0.11 | 10.0.5 | InMemory test provider compatibility |

   **Other Packages (No Update):**
   - Microsoft.NET.Test.Sdk 17.11.1 (compatible with .NET 10)
   - xunit 2.9.2 (compatible)
   - xunit.runner.visualstudio 2.8.2 (compatible)
   - FluentAssertions 6.12.2 (compatible)
   - Audacia.CodeAnalysis 1.5.1 (compatible)

4. **Expected Breaking Changes**
   - **Assessment Status:** No breaking changes detected (649 APIs analyzed, all compatible)
   - **Potential Areas to Monitor:**
     - EF Core InMemory provider behavior changes
     - Test SDK compatibility (expected to be seamless)
     - xunit runner behavior

5. **Code Modifications**
   - **Expected:** None (test infrastructure packages all compatible)
   - **If Needed:** Update test data setup if InMemory provider behavior changed
   - **Review Areas:**
     - Test database initialization (11 test files)
     - DbContext configuration in tests
     - Assertion patterns with FluentAssertions

6. **Testing Strategy**
   - **Execution:** Run full test suite after upgrade
   - **Expected Outcome:** All tests pass
   - **If Failures:** Categorize as setup issues vs. behavioral changes
   - **Validation:** Confirms entire solution works correctly on .NET 10

7. **Validation Checklist**
   - [ ] Project file updated to net10.0
   - [ ] Microsoft.EntityFrameworkCore updated to 10.0.5
   - [ ] Microsoft.EntityFrameworkCore.InMemory updated to 10.0.5
   - [ ] Dependencies on DataAccess, EntityFrameworkCore, SqlServer projects resolve correctly
   - [ ] Project builds without errors
   - [ ] Project builds without warnings
   - [ ] No NuGet package conflicts
   - [ ] All tests execute successfully
   - [ ] All tests pass (no regressions)
   - [ ] Test execution time acceptable

---

## Package Update Reference

### Overview

This section consolidates all package updates required across the solution for the All-At-Once migration.

### Package Updates by Scope

#### Entity Framework Core Packages (All Projects with EF Core)

**Projects Affected:** 5 projects
- Audacia.DataAccess.EntityFrameworkCore
- Audacia.DataAccess.EntityFrameworkCore.SqlServer
- Audacia.DataAccess.EntityFrameworkCore.Triggers
- Audacia.DataAccess.EntityFrameworkCore.Auditing
- Audacia.DataAccess.Tests

| Package | Current Version | Target Version | Projects | Update Reason |
|---------|----------------|----------------|----------|---------------|
| Microsoft.EntityFrameworkCore | 8.0.11 | 10.0.5 | 5 | .NET 10 framework compatibility, bug fixes, performance improvements |
| Microsoft.EntityFrameworkCore.Relational | 8.0.11 | 10.0.5 | 1 (Auditing) | Relational database feature compatibility |
| Microsoft.EntityFrameworkCore.SqlServer | 8.0.11 | 10.0.5 | 1 (SqlServer) | SQL Server provider compatibility |
| Microsoft.EntityFrameworkCore.InMemory | 8.0.11 | 10.0.5 | 1 (Tests) | InMemory test provider compatibility |

**Update Rationale:**
- EF Core 10.0.5 is the stable release for .NET 10
- All EF Core packages must be at same version to avoid conflicts
- Includes bug fixes and performance improvements from 8.0.11 → 10.0.5
- No breaking changes detected by assessment (3,555 APIs analyzed)

#### Packages with No Update Required

| Package | Version | Projects | Reason |
|---------|---------|----------|--------|
| Audacia.CodeAnalysis | 1.5.1 | 8 | ✅ Compatible with .NET 10 |
| Audacia.CodeAnalysis.Analyzers.Helpers | 1.1.2 | 1 | ✅ Compatible with .NET 10 |
| Audacia.Commands | 1.1.1 | 1 | ✅ Compatible with .NET 10 |
| Audacia.Core | 1.1.2 | 6 | ✅ Compatible with .NET 10 |
| FluentAssertions | 6.12.2 | 1 | ✅ Compatible with .NET 10 |
| Microsoft.NET.Test.Sdk | 17.11.1 | 1 | ✅ Compatible with .NET 10 |
| xunit | 2.9.2 | 1 | ✅ Compatible with .NET 10 |
| xunit.runner.visualstudio | 2.8.2 | 1 | ✅ Compatible with .NET 10 |

### Package Removals (Framework-Included Functionality)

**Projects Affected:** 2 projects
- Audacia.DataAccess.EntityFrameworkCore.Triggers
- Audacia.DataAccess.EntityFrameworkCore.Auditing

| Package | Current Version | Removal Reason |
|---------|----------------|----------------|
| System.Net.Http | 4.3.4 | Functionality is now part of .NET 10 runtime - no separate package needed |
| System.Text.RegularExpressions | 4.3.1 | Functionality is now part of .NET 10 runtime - no separate package needed |

**Removal Safety:**
- These packages were originally needed for .NET Standard/.NET Framework compatibility
- .NET 8+ includes these APIs natively in the framework
- No code changes required - APIs remain available through framework reference
- Removing prevents potential version conflicts with framework implementation

### Update Execution Strategy

**All-At-Once Approach:**

1. **Simultaneous Updates:** All 4 package updates applied across all affected projects in single operation
2. **Version Consistency:** Ensures all EF Core packages at same version (10.0.5) from the start
3. **Clean Removals:** Both System.* packages removed simultaneously from both projects
4. **Single Restore:** One `dotnet restore` operation after all updates complete

**Update Order (Internal):**
While updates are applied atomically, the build system processes them in this logical order:
1. Remove System.Net.Http and System.Text.RegularExpressions from Triggers and Auditing projects
2. Update Microsoft.EntityFrameworkCore across all 5 affected projects
3. Update Microsoft.EntityFrameworkCore.Relational in Auditing project
4. Update Microsoft.EntityFrameworkCore.SqlServer in SqlServer project
5. Update Microsoft.EntityFrameworkCore.InMemory in Tests project
6. Restore all packages
7. Build solution

### Package Version Verification

After updates, verify consistency with:

```bash
dotnet list package
```

**Expected Output:**
- All EF Core packages show version 10.0.5
- No version conflicts reported
- No deprecated package warnings
- System.Net.Http and System.Text.RegularExpressions absent from package lists

### Package Update Checklist

- [ ] Microsoft.EntityFrameworkCore → 10.0.5 in 5 projects
- [ ] Microsoft.EntityFrameworkCore.Relational → 10.0.5 in Auditing project
- [ ] Microsoft.EntityFrameworkCore.SqlServer → 10.0.5 in SqlServer project
- [ ] Microsoft.EntityFrameworkCore.InMemory → 10.0.5 in Tests project
- [ ] System.Net.Http removed from Triggers project
- [ ] System.Net.Http removed from Auditing project
- [ ] System.Text.RegularExpressions removed from Triggers project
- [ ] System.Text.RegularExpressions removed from Auditing project
- [ ] `dotnet restore` completes successfully
- [ ] No package downgrade warnings
- [ ] No version conflict warnings

---

## Risk Management

### High-Level Risk Assessment

**Overall Risk Level:** 🟢 **Low**

This migration has an exceptionally low risk profile due to:
- Small, well-structured solution
- No breaking changes detected in assessment
- All packages have stable .NET 10 versions
- Comprehensive test coverage
- All projects already SDK-style
- No security vulnerabilities

### Risk Factor Analysis

| Risk Category | Level | Description | Mitigation |
|--------------|-------|-------------|------------|
| **Framework Compatibility** | 🟢 Low | .NET 8 → .NET 10 (two LTS versions apart) | All 3,555 APIs analyzed show as compatible |
| **Package Compatibility** | 🟢 Low | All 4 packages have clear upgrade paths | EF Core 10.0.5 stable, widely adopted |
| **Breaking Changes** | 🟢 Low | Zero breaking changes detected | Assessment found no binary/source incompatibilities |
| **Build Complexity** | 🟢 Low | All SDK-style, no custom build logic | Standard MSBuild process |
| **Test Coverage** | 🟢 Low | Test project present covering core scenarios | Run full test suite after upgrade |
| **Dependency Conflicts** | 🟢 Low | Clean dependency tree, no circular refs | Build system handles resolution automatically |
| **Rollback Complexity** | 🟢 Low | Git branch allows easy revert | Single atomic commit enables clean rollback |

### All-At-Once Strategy Risk Factors

**Specific Risks of Atomic Upgrade:**

1. **Simultaneous Changes Across All Projects**
   - **Risk:** If something fails, all 8 projects affected
   - **Mitigation:** Low overall complexity + comprehensive tests catch issues early
   - **Fallback:** Git revert entire branch to pre-upgrade state

2. **Larger Testing Surface**
   - **Risk:** Must validate entire solution at once
   - **Mitigation:** Only 495 LOC in test project, manageable scope
   - **Fallback:** Test project structure allows targeted test execution if needed

3. **Coordination Required**
   - **Risk:** All developers must adapt simultaneously
   - **Mitigation:** Small team (inferred from solution size), clear communication
   - **Fallback:** Feature branch keeps master stable during migration

### Security Vulnerability Assessment

**Status:** ✅ **No vulnerabilities detected**

The assessment found zero security vulnerabilities in current packages. This upgrade proactively moves to newer framework/packages, maintaining security posture.

### Contingency Plans

#### If Build Fails After Project Updates

**Scenario:** Compilation errors after updating TargetFramework

**Actions:**
1. Review compiler error messages for specific API issues
2. Check if any transitive dependencies are incompatible
3. Consult breaking changes documentation for .NET 9 and .NET 10
4. Address errors iteratively, focusing on foundation projects first
5. If systematic issue found, document and assess rollback

**Likelihood:** Very Low (no breaking changes detected)

#### If Package Updates Cause Conflicts

**Scenario:** NuGet restore fails or package incompatibility

**Actions:**
1. Review NuGet dependency tree (`dotnet list package --include-transitive`)
2. Check for version constraints in Directory.Build.props or .csproj files
3. Verify all EF Core packages at same version (10.0.5)
4. Update any transitive dependency constraints if needed
5. Consult EF Core 10 release notes for package changes

**Likelihood:** Very Low (assessment validated package compatibility)

#### If Tests Fail After Upgrade

**Scenario:** Build succeeds but tests fail

**Actions:**
1. Categorize failures: setup issues vs. behavioral changes
2. Check for EF Core InMemory provider behavior changes
3. Review test configuration and database initialization
4. Verify test package versions compatible (xunit 2.9.2 compatible with .NET 10)
5. Fix behavioral changes or update test expectations as needed

**Likelihood:** Low (no behavioral changes detected, but tests validate assumptions)

#### If Performance Degrades

**Scenario:** Solution builds/runs slower after upgrade

**Actions:**
1. Benchmark specific operations (EF Core queries, test execution time)
2. Review .NET 10 performance characteristics (typically improvements)
3. Check for unintentional algorithm changes in updated packages
4. Profile hotspots if degradation is significant
5. Report issues to framework team if systematic problem found

**Likelihood:** Very Low (.NET 10 generally faster than .NET 8)

### Rollback Strategy

**Approach:** Git branch revert

**Steps:**
1. If blocking issue identified, stop execution
2. Document the specific failure mode
3. Revert branch: `git reset --hard origin/master`
4. Assess root cause
5. Update plan with additional mitigations
6. Retry when ready

**Time to Rollback:** < 5 minutes

**All-At-Once Rollback Advantage:** Single atomic commit means clean rollback with no partially-upgraded state to manage.

---

## Breaking Changes Catalog

### Overview

**Good News:** The assessment analyzed **3,555 APIs** across all projects and found **zero breaking changes**.

### Assessment Results

| Change Category | Count | Impact |
|----------------|-------|--------|
| 🔴 Binary Incompatible | 0 | High - Would require code changes |
| 🟡 Source Incompatible | 0 | Medium - Would need re-compilation fixes |
| 🔵 Behavioral Changes | 0 | Low - Would require runtime testing |
| ✅ Compatible | 3,555 | No impact - seamless upgrade |

### Framework Breaking Changes (.NET 8 → .NET 10)

**Scope:** .NET 9 and .NET 10 combined breaking changes

**Status:** None detected in this codebase

**Monitoring Recommendation:** While assessment found no issues, be aware of these general .NET 10 areas:
- **Serialization:** System.Text.Json behavior changes (not used extensively in assessment)
- **Threading:** Task and async pattern updates (review if using advanced patterns)
- **Globalization:** Culture handling changes (review if using date/number formatting)
- **Security:** Cryptography API updates (not detected in codebase)

**Reference:** [.NET 10 Breaking Changes](https://learn.microsoft.com/en-us/dotnet/core/compatibility/10.0)

### Entity Framework Core Breaking Changes (8.0 → 10.0)

**Scope:** EF Core 9.0 and 10.0 combined breaking changes

**Status:** None detected in this codebase

**Known EF Core 10 Changes to Monitor:**
1. **Query Translation:** Some LINQ query patterns may translate differently (test coverage will catch)
2. **Change Tracking:** Internal change tracking optimizations (transparent to most code)
3. **Migrations:** Migration generation may produce slightly different SQL (review migrations if regenerated)
4. **Interceptors:** Interceptor APIs enhanced but backward compatible
5. **Logging:** Logging event IDs and messages may have changed (review logging if tightly coupled)

**Reference:** [EF Core 10 Breaking Changes](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-10.0/breaking-changes)

### Package Removal Breaking Changes

**Removed Packages:**
- System.Net.Http 4.3.4
- System.Text.RegularExpressions 4.3.1

**Impact:** ✅ None - Functionality remains available through .NET 10 framework

**Rationale:**
- These packages provided APIs for older .NET Standard libraries
- .NET 10 includes these APIs natively in the base class library
- Same namespaces, same method signatures - no code changes required
- Removing package references eliminates potential version conflicts

### API Usage Patterns

**Assessment Findings by Project:**

| Project | APIs Analyzed | Incompatibilities | Notes |
|---------|--------------|-------------------|-------|
| Audacia.DataAccess | 908 | 0 | Largest API surface, all compatible |
| Audacia.DataAccess.EntityFrameworkCore.Auditing | 896 | 0 | Complex EF Core usage, all compatible |
| Audacia.DataAccess.Tests | 649 | 0 | Test infrastructure compatible |
| Audacia.DataAccess.EntityFrameworkCore.SqlServer | 422 | 0 | SQL Server provider compatible |
| Audacia.DataAccess.EntityFrameworkCore.Triggers | 406 | 0 | Trigger APIs compatible |
| Audacia.DataAccess.EntityFrameworkCore | 200 | 0 | Core EF APIs compatible |
| Audacia.DataAccess.Commands | 46 | 0 | Command patterns compatible |
| Audacia.DataAccess.Model | 28 | 0 | Simple models compatible |

### Behavioral Changes to Monitor

While no breaking changes detected, monitor these areas during testing:

#### Entity Framework Core Behavior

1. **Query Execution**
   - **Area:** LINQ to SQL translation
   - **Potential Change:** More aggressive query simplification
   - **Detection:** Compare test query results
   - **Likelihood:** Very Low

2. **Change Tracking**
   - **Area:** Entity state management
   - **Potential Change:** Optimized tracking algorithms
   - **Detection:** Audit trail generation tests
   - **Likelihood:** Very Low

3. **InMemory Provider (Tests)**
   - **Area:** Test database behavior
   - **Potential Change:** InMemory provider semantics
   - **Detection:** Test failures after upgrade
   - **Likelihood:** Low

#### Framework Behavior

1. **Async Operations**
   - **Area:** Task and ValueTask usage
   - **Potential Change:** Scheduling optimizations
   - **Detection:** Timing-sensitive tests (if any)
   - **Likelihood:** Very Low

2. **Collection Enumeration**
   - **Area:** LINQ operation ordering
   - **Potential Change:** Iteration optimizations
   - **Detection:** Tests expecting specific ordering
   - **Likelihood:** Very Low

### Deprecation Warnings

**Expected:** None based on assessment

**If Encountered:**
- Note the API and deprecated message
- Consult .NET 10 documentation for replacement API
- Plan updates for future .NET release (not blocking)
- Most deprecations have 2+ release grace period

### Breaking Change Response Plan

**If Unexpected Breaking Change Found:**

1. **Identify:**
   - Note exact error message or behavior change
   - Identify affected project and code location
   - Determine if blocking or warning

2. **Research:**
   - Search .NET 10 breaking changes documentation
   - Check EF Core 10 release notes
   - Review GitHub issues for similar reports

3. **Resolve:**
   - Apply recommended fix from documentation
   - Update code to use new API or pattern
   - Add comment explaining .NET 10 requirement

4. **Validate:**
   - Rebuild affected project
   - Run affected tests
   - Verify fix doesn't introduce new issues

5. **Document:**
   - Update this plan with discovered breaking change
   - Note resolution for future reference
   - Share with team if applicable

### Summary

**Expected Breaking Changes:** 0
**Detected Breaking Changes:** 0
**Mitigation Required:** None

The upgrade from .NET 8 to .NET 10 is **exceptionally clean** for this codebase. Proceed with high confidence.

---

## Testing & Validation Strategy

### Overview

This section defines the multi-level testing approach to validate the All-At-Once migration from .NET 8 to .NET 10.

### Testing Philosophy

**All-At-Once Testing Approach:**
- No per-project testing (all projects migrate simultaneously)
- Single comprehensive validation after atomic upgrade
- Full solution builds and tests together
- Pass/fail at solution level, not project level

### Testing Levels

#### Level 1: Build Validation

**Objective:** Ensure entire solution compiles successfully on .NET 10

**Scope:** All 8 projects

**Validation Steps:**

1. **Clean Build**
   ```bash
   dotnet clean
   dotnet restore
   dotnet build --configuration Release
   ```

2. **Success Criteria:**
   - ✅ All 8 projects build without errors
   - ✅ All 8 projects build without warnings
   - ✅ No NuGet package restore conflicts
   - ✅ No transitive dependency conflicts
   - ✅ Build time comparable to .NET 8 baseline

3. **Expected Output:**
   ```
   Build succeeded.
       0 Warning(s)
       0 Error(s)
   ```

4. **If Build Fails:**
   - Review compiler error messages
   - Check for missing package references
   - Verify all TargetFramework properties updated
   - Consult Breaking Changes Catalog
   - Apply fixes iteratively
   - Rebuild and verify

#### Level 2: Unit Test Execution

**Objective:** Validate functionality through automated test suite

**Scope:** Audacia.DataAccess.Tests project (495 LOC, 11 test files)

**Test Execution:**

```bash
dotnet test tests\Audacia.DataAccess.Tests\Audacia.DataAccess.Tests.csproj --configuration Release
```

**Success Criteria:**
- ✅ All tests pass (no failures)
- ✅ No tests skipped unexpectedly
- ✅ Test execution time acceptable (within 20% of baseline)
- ✅ No test infrastructure errors
- ✅ Code coverage maintained (if measured)

**Test Coverage Areas:**
Based on project dependencies, tests likely cover:
- Data access abstractions (Audacia.DataAccess)
- EF Core implementations (Audacia.DataAccess.EntityFrameworkCore)
- SQL Server provider functionality (Audacia.DataAccess.EntityFrameworkCore.SqlServer)
- Repository patterns
- Query specifications
- Unit of work patterns

**If Tests Fail:**
1. **Categorize Failures:**
   - Setup/teardown issues (test infrastructure)
   - Behavioral changes (EF Core InMemory provider)
   - API compatibility (unexpected breaking changes)
   - Timing/concurrency issues (rare)

2. **Triage by Severity:**
   - **Critical:** Core functionality broken (blocker)
   - **Moderate:** Edge case behavior changed (investigate)
   - **Low:** Test assumptions invalid (update test)

3. **Resolution Steps:**
   - Review test failure messages and stack traces
   - Check EF Core 10 InMemory provider changes
   - Compare expected vs. actual behavior
   - Update test expectations if behavior change is valid
   - Fix implementation code if regression found

#### Level 3: Package Verification

**Objective:** Ensure package ecosystem is healthy

**Validation Commands:**

```bash
# List all packages and versions
dotnet list package

# Check for deprecated packages
dotnet list package --deprecated

# Check for vulnerable packages
dotnet list package --vulnerable

# Check for outdated packages
dotnet list package --outdated
```

**Success Criteria:**
- ✅ All EF Core packages at version 10.0.5
- ✅ No deprecated packages reported
- ✅ No vulnerable packages reported
- ✅ No unexpected package downgrades
- ✅ System.Net.Http absent from Triggers and Auditing
- ✅ System.Text.RegularExpressions absent from Triggers and Auditing

#### Level 4: Solution-Wide Validation

**Objective:** Comprehensive solution health check

**Validation Steps:**

1. **Rebuild Entire Solution**
   ```bash
   dotnet clean
   dotnet build --configuration Release --no-incremental
   ```

2. **Run All Tests**
   ```bash
   dotnet test --configuration Release --verbosity normal
   ```

3. **Check for Warnings**
   - Review build output for any warnings
   - Address obsolete API warnings (document for future)
   - Fix any code analysis warnings

4. **Verify Project References**
   - All project-to-project references resolve correctly
   - No version mismatches between projects
   - Dependency graph remains valid

**Success Criteria:**
- ✅ Solution builds cleanly
- ✅ All tests pass
- ✅ No warnings in release build
- ✅ No version conflicts
- ✅ All projects targeting net10.0

### Testing Checklist

#### Pre-Upgrade Baseline

- [ ] Record current build time
- [ ] Record current test execution time
- [ ] Record current test pass count
- [ ] Ensure all tests passing on .NET 8
- [ ] Commit current state to source control

#### Post-Upgrade Validation

**Build Validation:**
- [ ] `dotnet restore` succeeds
- [ ] `dotnet build` succeeds (all projects)
- [ ] 0 build errors
- [ ] 0 build warnings
- [ ] Build time acceptable

**Package Validation:**
- [ ] All EF Core packages at 10.0.5
- [ ] No deprecated packages
- [ ] No vulnerable packages
- [ ] System.* packages removed correctly

**Test Validation:**
- [ ] `dotnet test` succeeds
- [ ] All tests pass (no failures)
- [ ] No unexpected skipped tests
- [ ] Test execution time acceptable
- [ ] No test infrastructure errors

**Solution Validation:**
- [ ] Clean rebuild succeeds
- [ ] All project references resolve
- [ ] No transitive dependency conflicts
- [ ] Release build produces expected outputs

### Smoke Testing Guidance

While comprehensive test suite provides primary validation, consider these manual checks if applicable to consuming applications:

**Not Automatable (Informational Only):**
- Basic application startup (if web application consumers exist)
- Database connectivity (if real database configuration present)
- Logging output review (ensure no unexpected error logs)
- Performance spot-check (compare response times)

**Note:** These are informational observations, not required automation tasks.

### Rollback Criteria

**Trigger Rollback If:**
- ❌ Build fails with unresolvable errors
- ❌ More than 10% of tests fail
- ❌ Critical functionality broken
- ❌ Unresolvable package conflicts
- ❌ Severe performance degradation (>50% slower)

**Do NOT Rollback For:**
- ✅ Single test failure (can be fixed)
- ✅ Deprecation warnings (not blocking)
- ✅ Minor behavioral differences (validate expected)
- ✅ Cosmetic warning messages

### Performance Validation

**Metrics to Compare (.NET 8 baseline vs. .NET 10):**

1. **Build Time:**
   - Measure: `dotnet build` duration
   - Expected: Comparable or faster
   - Threshold: Within 20% of baseline

2. **Test Execution Time:**
   - Measure: `dotnet test` duration
   - Expected: Comparable or faster
   - Threshold: Within 20% of baseline

3. **Package Restore Time:**
   - Measure: `dotnet restore` duration
   - Expected: Comparable
   - Threshold: Within 30% of baseline

**If Performance Degrades:**
- Investigate EF Core query translation changes
- Review for unintentional algorithm changes
- Profile hotspots with performance tools
- Consult .NET 10 performance documentation
- Consider reporting issue to framework team

### Success Validation Summary

**Migration Succeeds When ALL These Are True:**

✅ **Build:** Solution builds without errors or warnings  
✅ **Tests:** All tests pass with no failures  
✅ **Packages:** All packages at correct versions, no conflicts  
✅ **Performance:** Build and test times acceptable  
✅ **Dependencies:** All project references resolve correctly  
✅ **Security:** No vulnerable packages detected  

**With All-At-Once Strategy:** These must all be true simultaneously - no partial success states.

---

## Complexity & Effort Assessment

### Per-Project Complexity Ratings

| Project | Complexity | LOC | Files | Dependencies | Package Updates | Risk Factors |
|---------|-----------|-----|-------|--------------|-----------------|--------------|
| Audacia.DataAccess.Model | Low | 99 | 5 | 0 | 0 | None |
| Audacia.DataAccess | Low | 2,454 | 41 | 0 | 0 | Largest codebase, 4 dependants |
| Audacia.DataAccess.Commands | Low | 82 | 1 | 1 | 0 | Smallest codebase |
| Audacia.DataAccess.EntityFrameworkCore | Low | 307 | 5 | 1 | 1 | EF Core package update |
| Audacia.DataAccess.EntityFrameworkCore.SqlServer | Low | 687 | 2 | 1 | 2 | EF Core + provider updates |
| Audacia.DataAccess.EntityFrameworkCore.Triggers | Low | 788 | 8 | 1 | 3 | EF Core + 2 package removals |
| Audacia.DataAccess.EntityFrameworkCore.Auditing | Low | 1,598 | 19 | 2 | 4 | EF Core + Relational + 2 removals |
| Audacia.DataAccess.Tests | Low | 495 | 11 | 3 | 2 | EF Core + InMemory updates |

### Phase Complexity Assessment

**Single Atomic Phase:** All-At-Once Upgrade

**Complexity Rating:** Low

**Contributing Factors:**
- All projects marked Low difficulty by assessment
- No API breaking changes detected
- All package updates are straightforward version bumps
- Clear dependency structure simplifies build validation
- Package removals are safe (functionality now in framework)

**Effort Drivers:**
1. **Project File Updates:** Straightforward TargetFramework changes (8 files)
2. **Package Updates:** 4 EF Core packages upgrading 8.0.11 → 10.0.5
3. **Package Removals:** 2 packages (System.Net.Http, System.Text.RegularExpressions) from 2 projects
4. **Build Validation:** Single solution build
5. **Test Execution:** Single test project run

**Complexity Mitigators:**
- No SDK conversion needed (all projects already SDK-style)
- No multi-targeting required (straight migration)
- No architectural changes needed
- No custom build scripts or complex MSBuild logic visible
- Comprehensive test coverage available for validation

### Resource Requirements

**Skills Required:**
- Understanding of .NET SDK project structure
- Familiarity with Entity Framework Core versioning
- Basic MSBuild/NuGet knowledge
- Ability to interpret compiler errors (if any arise)

**Skill Level:** Intermediate .NET Developer

**Parallel Capacity:** Not applicable (single atomic operation)

**Estimated Relative Effort:** Low

The All-At-Once approach means this is treated as **one cohesive effort** rather than 8 separate project efforts. The relative effort is low due to:
- Small solution size
- No identified complications
- Straightforward framework and package updates
- Comprehensive test suite for validation

---

## Source Control Strategy

### Overview

This section defines the Git branching, commit, and merge strategy for the All-At-Once .NET 10 migration.

### Branching Strategy

**Branch Structure:**

```
master (source branch - .NET 8)
  └── upgrade-to-NET10 (upgrade branch - .NET 10)
```

**Branch Details:**

- **Source Branch:** `master`
  - Current state: All projects on .NET 8
  - Status: Stable, production-ready
  - Protected: No direct upgrades applied here

- **Upgrade Branch:** `upgrade-to-NET10`
  - Purpose: Isolated .NET 10 migration work
  - Created from: `master` at migration start
  - Isolated: Changes don't affect `master` until merged
  - Disposable: Can be deleted and recreated if major issues

**Branch Creation:**

Already created:
```bash
git checkout master
git pull origin master
git checkout -b upgrade-to-NET10
git push -u origin upgrade-to-NET10
```

### Commit Strategy

**All-At-Once Commit Approach:**

**Preferred: Single Atomic Commit**

Given the All-At-Once strategy, prefer **one comprehensive commit** containing all changes:

**Commit Structure:**
```
Upgrade solution to .NET 10

- Update all 8 projects from net8.0 to net10.0
- Update EntityFrameworkCore packages from 8.0.11 to 10.0.5
- Remove System.Net.Http and System.Text.RegularExpressions (now in framework)
- All projects build successfully
- All tests pass

Projects updated:
- Audacia.DataAccess.Model
- Audacia.DataAccess
- Audacia.DataAccess.Commands
- Audacia.DataAccess.EntityFrameworkCore
- Audacia.DataAccess.EntityFrameworkCore.SqlServer
- Audacia.DataAccess.EntityFrameworkCore.Triggers
- Audacia.DataAccess.EntityFrameworkCore.Auditing
- Audacia.DataAccess.Tests

Package updates:
- Microsoft.EntityFrameworkCore: 8.0.11 → 10.0.5 (5 projects)
- Microsoft.EntityFrameworkCore.Relational: 8.0.11 → 10.0.5 (Auditing)
- Microsoft.EntityFrameworkCore.SqlServer: 8.0.11 → 10.0.5 (SqlServer)
- Microsoft.EntityFrameworkCore.InMemory: 8.0.11 → 10.0.5 (Tests)

Packages removed:
- System.Net.Http from Triggers and Auditing
- System.Text.RegularExpressions from Triggers and Auditing
```

**Commit Command:**
```bash
git add .
git commit -m "Upgrade solution to .NET 10

[detailed message as above]"
git push origin upgrade-to-NET10
```

**Alternative: Two-Commit Approach (If Needed)**

If execution requires separation for validation:

**Commit 1: Project and Package Updates**
```
Upgrade projects to .NET 10 and update packages

- Update all 8 projects from net8.0 to net10.0
- Update EntityFrameworkCore packages from 8.0.11 to 10.0.5
- Remove System.Net.Http and System.Text.RegularExpressions
```

**Commit 2: Compilation Fixes (If Any)**
```
Fix compilation errors from .NET 10 upgrade

- [List specific fixes applied]
- All projects now build successfully
```

**When to Use Two-Commit:**
- If unexpected compilation errors require fixes
- If fixes are substantial enough to warrant separate commit
- If you want to preserve pre-fix state for reference

### Commit Best Practices

**Do:**
- ✅ Commit only after validation passes (build succeeds, tests pass)
- ✅ Include comprehensive commit message
- ✅ List all projects and packages changed
- ✅ Note validation status (builds/tests pass)
- ✅ Keep atomic: all related changes together

**Don't:**
- ❌ Commit with build errors
- ❌ Commit with test failures
- ❌ Split project updates across multiple commits (breaks atomicity)
- ❌ Use generic messages like "upgrade" without details

### Review and Merge Process

**Pull Request Creation:**

After successful validation on `upgrade-to-NET10` branch:

1. **Create PR:**
   - From: `upgrade-to-NET10`
   - To: `master`
   - Title: `Upgrade solution to .NET 10 (LTS)`

2. **PR Description Template:**
   ```markdown
   ## Overview
   Upgrades entire Audacia.DataAccess solution from .NET 8 to .NET 10 (LTS) using All-At-Once strategy.

   ## Changes
   - All 8 projects upgraded to net10.0
   - Entity Framework Core packages upgraded to 10.0.5
   - Removed System.Net.Http and System.Text.RegularExpressions (now in framework)

   ## Validation
   - ✅ All projects build without errors or warnings
   - ✅ All tests pass (0 failures)
   - ✅ No package conflicts
   - ✅ No security vulnerabilities

   ## Breaking Changes
   None detected - all 3,555 APIs compatible.

   ## Testing
   - Build time: [X seconds] (baseline: [Y seconds])
   - Test execution: [X seconds] (baseline: [Y seconds])
   - Test pass rate: 100%

   ## Rollback Plan
   Revert merge commit if issues discovered post-merge.

   ## References
   - Migration plan: `.github/upgrades/scenarios/new-dotnet-version_0ffae4/plan.md`
   - Assessment: `.github/upgrades/scenarios/new-dotnet-version_0ffae4/assessment.md`
   ```

**PR Review Checklist:**

Reviewers should verify:

- [ ] All 8 project files updated to net10.0
- [ ] All EF Core packages at version 10.0.5
- [ ] System.Net.Http and System.Text.RegularExpressions removed
- [ ] No unintended file changes
- [ ] Build succeeds on reviewer machine
- [ ] Tests pass on reviewer machine
- [ ] No code quality regressions
- [ ] Commit message is descriptive

**Merge Strategy:**

**Recommended:** Squash merge or regular merge (single commit)

```bash
# After PR approval
git checkout master
git pull origin master
git merge --no-ff upgrade-to-NET10 -m "Merge: Upgrade solution to .NET 10"
git push origin master
```

**Post-Merge:**
```bash
# Optionally delete upgrade branch
git branch -d upgrade-to-NET10
git push origin --delete upgrade-to-NET10
```

### Rollback After Merge

**If Issues Discovered Post-Merge:**

**Option 1: Revert Merge Commit**
```bash
git checkout master
git revert -m 1 HEAD  # Revert the merge commit
git push origin master
```

**Option 2: Hot-Fix on New Branch**
```bash
git checkout -b hotfix/dotnet10-issue
# Apply fix
git commit -m "Fix: [description]"
# Create PR to master
```

**Decision Criteria:**
- Use Option 1 (revert) if: Critical blocker, fix requires significant work
- Use Option 2 (hot-fix) if: Minor issue, quick fix available

### Branch Protection

**Recommended Settings for `master`:**

- ✅ Require pull request before merging
- ✅ Require status checks to pass (if CI/CD configured)
- ✅ Require branches to be up to date
- ✅ Require review from code owners
- ❌ Allow force pushes: Disabled

**For `upgrade-to-NET10`:**

- Less restrictive: allows iterative development
- Can be force-pushed if restarts needed
- Not production branch, can be recreated

### All-At-Once Source Control Benefits

**Single Commit Advantages:**

1. **Clean History:** One commit represents entire migration
2. **Easy Rollback:** Revert one commit to undo everything
3. **Clear Audit Trail:** All changes visible in one diff
4. **Simple Cherry-Pick:** Can apply to other branches if needed
5. **Atomic State:** No intermediate partially-upgraded commits

**Why This Works for All-At-Once:**

- All projects change together (matches strategy)
- Single validation point (all tests pass)
- No multi-commit coordination needed
- Simplifies code review (one PR to review)

### Git Best Practices Summary

**Do:**
- ✅ Use descriptive branch names
- ✅ Commit after validation passes
- ✅ Write comprehensive commit messages
- ✅ Create detailed pull requests
- ✅ Keep `master` stable

**Don't:**
- ❌ Commit broken builds
- ❌ Push failing tests
- ❌ Use vague commit messages
- ❌ Skip code review
- ❌ Force-push to `master`

### Integration with CI/CD

**If CI/CD Pipeline Exists:**

Ensure pipeline runs:
1. **On PR Creation:** Validates `upgrade-to-NET10` branch
2. **On PR Update:** Re-validates after changes
3. **Before Merge:** Final validation on merge candidate

**Pipeline Should:**
- Build solution on .NET 10 SDK
- Run all tests
- Check for package vulnerabilities
- Enforce code quality standards
- Report results back to PR

**If Pipeline Fails:**
- Fix issues on `upgrade-to-NET10` branch
- Push updated commits
- Wait for pipeline to re-validate
- Merge only after green pipeline

---

## Success Criteria

### Overview

This migration is considered **complete and successful** when all criteria below are met simultaneously. With the All-At-Once strategy, there are no intermediate success states - the solution is either fully migrated or not migrated.

### Technical Criteria

#### ✅ Framework Migration Complete

**All Projects Targeting .NET 10:**

- [ ] Audacia.DataAccess.Model.csproj → net10.0
- [ ] Audacia.DataAccess.csproj → net10.0
- [ ] Audacia.DataAccess.Commands.csproj → net10.0
- [ ] Audacia.DataAccess.EntityFrameworkCore.csproj → net10.0
- [ ] Audacia.DataAccess.EntityFrameworkCore.SqlServer.csproj → net10.0
- [ ] Audacia.DataAccess.EntityFrameworkCore.Triggers.csproj → net10.0
- [ ] Audacia.DataAccess.EntityFrameworkCore.Auditing.csproj → net10.0
- [ ] Audacia.DataAccess.Tests.csproj → net10.0

**Verification:**
```bash
# All projects should show <TargetFramework>net10.0</TargetFramework>
grep -r "TargetFramework" src/**/*.csproj tests/**/*.csproj
```

#### ✅ Package Updates Applied

**Entity Framework Core Packages Updated:**

- [ ] Microsoft.EntityFrameworkCore → 10.0.5 (5 projects)
- [ ] Microsoft.EntityFrameworkCore.Relational → 10.0.5 (Auditing)
- [ ] Microsoft.EntityFrameworkCore.SqlServer → 10.0.5 (SqlServer)
- [ ] Microsoft.EntityFrameworkCore.InMemory → 10.0.5 (Tests)

**Verification:**
```bash
dotnet list package | grep "Microsoft.EntityFrameworkCore"
# All versions should show 10.0.5
```

#### ✅ Package Removals Complete

**Framework-Included Packages Removed:**

- [ ] System.Net.Http removed from Triggers project
- [ ] System.Net.Http removed from Auditing project
- [ ] System.Text.RegularExpressions removed from Triggers project
- [ ] System.Text.RegularExpressions removed from Auditing project

**Verification:**
```bash
dotnet list package | grep "System.Net.Http"
dotnet list package | grep "System.Text.RegularExpressions"
# Should return no results
```

#### ✅ Build Success

**Solution Builds Without Errors:**

- [ ] `dotnet restore` completes successfully
- [ ] `dotnet build` completes successfully
- [ ] 0 build errors
- [ ] 0 build warnings (or only expected deprecation warnings)
- [ ] All 8 projects compile
- [ ] All project references resolve correctly

**Verification:**
```bash
dotnet clean
dotnet restore
dotnet build --configuration Release
# Expected: Build succeeded. 0 Warning(s) 0 Error(s)
```

#### ✅ Test Success

**All Tests Pass:**

- [ ] `dotnet test` completes successfully
- [ ] All tests pass (0 failures)
- [ ] No tests skipped unexpectedly
- [ ] Test execution time acceptable (within 20% of baseline)
- [ ] No test infrastructure errors

**Verification:**
```bash
dotnet test --configuration Release
# Expected: Passed! - Total: X, Passed: X, Failed: 0, Skipped: 0
```

#### ✅ Package Ecosystem Health

**No Package Issues:**

- [ ] No deprecated packages
- [ ] No vulnerable packages
- [ ] No package version conflicts
- [ ] No transitive dependency issues
- [ ] All package restores succeed

**Verification:**
```bash
dotnet list package --deprecated
# Expected: No deprecated packages

dotnet list package --vulnerable
# Expected: No vulnerable packages

dotnet list package
# Expected: No conflicts or warnings
```

### Quality Criteria

#### ✅ Code Quality Maintained

**No Regressions:**

- [ ] Code analysis still passes (if configured)
- [ ] No new code quality warnings
- [ ] Existing code patterns preserved
- [ ] No unintended code changes

**Verification:**
```bash
dotnet build /p:TreatWarningsAsErrors=true
# Should succeed if no quality regressions
```

#### ✅ Test Coverage Maintained

**Coverage Metrics:**

- [ ] Test count unchanged (no tests lost)
- [ ] Test coverage percentage maintained (if measured)
- [ ] All existing test scenarios still covered
- [ ] Tests validate expected behavior

**Note:** Test coverage measurement depends on project tooling configuration.

#### ✅ Documentation Updated

**Migration Artifacts:**

- [ ] Assessment.md present and accurate
- [ ] Plan.md present and followed
- [ ] Commit messages descriptive
- [ ] PR description complete (if using PR workflow)

### Process Criteria

#### ✅ All-At-Once Strategy Followed

**Strategy Adherence:**

- [ ] All 8 projects updated simultaneously
- [ ] Single atomic upgrade operation completed
- [ ] No multi-targeting used
- [ ] No incremental project-by-project migration
- [ ] Dependencies respected by build system

#### ✅ Source Control Strategy Followed

**Git Workflow:**

- [ ] Changes on `upgrade-to-NET10` branch
- [ ] Atomic commit(s) applied
- [ ] Commit messages descriptive
- [ ] PR created (if using PR workflow)
- [ ] Code review completed (if required)
- [ ] Master branch protected

**Verification:**
```bash
git log --oneline upgrade-to-NET10 ^master
# Should show upgrade commit(s)
```

#### ✅ Validation Completed

**All Validation Levels Passed:**

- [ ] Level 1: Build validation ✓
- [ ] Level 2: Unit test execution ✓
- [ ] Level 3: Package verification ✓
- [ ] Level 4: Solution-wide validation ✓

### Acceptance Criteria (Checklist)

**Before Declaring Success, Verify ALL of These:**

**Framework & Packages:**
- [ ] All 8 projects on net10.0
- [ ] All EF Core packages at 10.0.5
- [ ] System.* packages removed
- [ ] No package conflicts

**Build & Tests:**
- [ ] Solution builds (0 errors)
- [ ] Solution builds (0 warnings)
- [ ] All tests pass
- [ ] Test count unchanged

**Quality:**
- [ ] No code quality regressions
- [ ] No security vulnerabilities
- [ ] No deprecated packages
- [ ] Documentation complete

**Process:**
- [ ] All-At-Once strategy followed
- [ ] Source control strategy followed
- [ ] Validation checklist complete
- [ ] Team informed of changes

### Success Validation Commands

**Run These Commands to Confirm Success:**

```bash
# 1. Verify target frameworks
grep -r "TargetFramework" src/**/*.csproj tests/**/*.csproj

# 2. Verify package versions
dotnet list package | grep "EntityFrameworkCore"

# 3. Verify removals
dotnet list package | grep "System.Net.Http"
dotnet list package | grep "System.Text.RegularExpressions"

# 4. Build verification
dotnet clean && dotnet build --configuration Release

# 5. Test verification
dotnet test --configuration Release

# 6. Package health
dotnet list package --deprecated
dotnet list package --vulnerable
```

**All commands should complete successfully with expected outputs.**

### Definition of Done

**The .NET 10 migration is DONE when:**

1. ✅ **All 8 projects** target net10.0
2. ✅ **All required packages** updated to version 10.0.5
3. ✅ **Unwanted packages** removed (System.Net.Http, System.Text.RegularExpressions)
4. ✅ **Solution builds** without errors or warnings
5. ✅ **All tests pass** with no failures
6. ✅ **No package issues** (deprecated, vulnerable, conflicts)
7. ✅ **Code quality maintained** (no regressions)
8. ✅ **Documentation complete** (plan, assessment, commits)
9. ✅ **Source control clean** (proper branch, commits, PR)
10. ✅ **Team informed** and ready to work with .NET 10

### Post-Migration Success Validation

**After Merge to Master:**

**Immediate Validation:**
- [ ] Pull master branch
- [ ] Rebuild solution
- [ ] Run tests
- [ ] Verify CI/CD pipeline passes (if configured)

**Ongoing Monitoring:**
- [ ] Watch for issues in consuming applications
- [ ] Monitor performance metrics
- [ ] Track any unexpected behavior
- [ ] Address any post-migration issues promptly

### Success Declaration

**Declare Success When:**

All technical, quality, and process criteria above are met, and:
- Solution successfully migrated to .NET 10
- All validation passed
- Team confirmed ready
- No blocking issues identified

**Communication:**

Notify team:
```
✅ .NET 10 Migration Complete

- All 8 projects upgraded to net10.0
- All packages updated successfully  
- All tests passing
- Solution ready for development

Branch: upgrade-to-NET10
Merged to: master (if merged)
Status: ✅ SUCCESS
```

### Failure Criteria (When NOT to Declare Success)

**Do NOT declare success if ANY of these are true:**

- ❌ Any project not on net10.0
- ❌ Build fails
- ❌ Any tests fail
- ❌ Package conflicts exist
- ❌ Security vulnerabilities detected
- ❌ Significant performance degradation (>50%)
- ❌ Critical functionality broken
- ❌ Team not ready to adopt .NET 10

**If failure criteria met:** Execute rollback plan, reassess, fix issues, retry migration.

---

**Migration Plan Complete**

This plan provides comprehensive guidance for upgrading the Audacia.DataAccess solution from .NET 8 to .NET 10 using the All-At-Once strategy. Follow the plan step-by-step, validate at each level, and declare success only when all criteria are met.
