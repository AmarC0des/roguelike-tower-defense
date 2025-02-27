# Contributing to the Roguelike Game Project

Welcome to the Roguelike Game Project! 🎮 We appreciate your contributions and collaboration. Please follow the guidelines below to maintain a smooth workflow.

---

## 🏗️ Branching Strategy

We follow a structured Git workflow:

1. **Feature Development**
   - Create a feature branch from `develop`:
     ```sh
     git checkout develop
     git pull origin develop
     git checkout -b feature/your-feature-name
     ```
   - Keep commits small and meaningful.

2. **Merging to Develop**
   - Open a **Pull Request (PR)** from `feature/your-feature-name` to `develop`.
   - PRs require **2 approvals** before merging.
   - Ensure all tests pass before merging.

3. **Merging to Main**
   - The `main` branch is for production-ready builds.
   - Only merge `develop` into `main` when a stable release is ready.
   - PRs to `main` require **at least 2 approvals**.

---

## 📌 Commit Message Guidelines

We follow **conventional commits** to maintain clarity:

- **feat:** New feature
- **fix:** Bug fix
- **refactor:** Code improvement without changing behavior
- **docs:** Documentation updates
- **test:** Adding/updating tests
- **chore:** Maintenance tasks (e.g., CI/CD updates)

Example:
```sh
git commit -m "feat: added enemy AI pathfinding"
```

---

## 🔄 Pull Request Procedure

1. Ensure your branch is **up to date** with `develop` before opening a PR.
   ```sh
   git pull origin develop
   ```

2. Open a **Pull Request (PR)** with:
   - A **descriptive title** summarizing the changes.
   - A **detailed description** explaining what was added, fixed, or improved.

3. Assign at least **one reviewer** to the PR.

4. Ensure the following before merging:
   - ✅ **All tests pass.**
   - ✅ **Code follows the project's style guidelines.**
   - ✅ **The PR is linked to a Kanban board task.**

---

## ✅ Testing Requirements

All contributions **must** pass tests before merging. Our test structure includes:

- **Unit Tests** (`Assets/Tests/`)  
- **Integration Tests** (for complex interactions)  
- **Playtests** (for user experience validation)  

### **To run tests in Unity:**
1. Open **Unity Editor**.
2. Navigate to **`Window > General > Test Runner`**.
3. Run **all tests** and ensure they pass.

For **CI/CD**, tests **must pass** before merging.

---

## 📋 Kanban Board Workflow

We use a **Kanban board** to track development:

1. Pick an **open task** from the **"To Do"** column.
2. Move the task to **"In Progress"** when you start working.
3. Move the task to **"In Review"** when opening a PR.
4. Once merged, move it to **"Done"**.

---

## ⚠️ Code Style & Best Practices

- Follow **C# best practices** for Unity.
- Use **camelCase** for variables, **PascalCase** for methods and classes.
- Write **clean, modular, and reusable** code.
- Document functions where necessary.

---

## ❓ Need Help?

If you have questions, create a **GitHub Discussion** or ask in the team chat.

Happy coding! 🚀