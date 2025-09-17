# 查看当前改动状态
git status

# 添加所有改动到暂存区
git add .

# 提交改动（带描述）
git commit -m "这里写提交说明，比如：修复血量系统bug"

# 推送到 GitHub
git push

# 从 GitHub 拉取最新代码（保持和远程同步）
git pull

# 查看提交历史（简洁模式）
git log --oneline

# 切换到某个旧版本（只查看，不建议开发）
git checkout <提交ID>

# 回到最新 main 分支
git checkout main

# 真正回退到某个版本（会丢失之后的提交，慎用）
git reset --hard <提交ID>

# 创建并切换到新分支（例如开发新功能）
git checkout -b feature-xxx

# 切回 main 分支
git checkout main

# 合并分支到 main
git merge feature-xxx

# 删除分支
git branch -d feature-xxx

