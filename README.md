### 将军之影

**演示视频地址：**

https://www.bilibili.com/video/BV1P6yiYtEH5

**项目介绍：**游戏类型是2D横板动作游戏，玩家将扮演将军的得力干将，踏上平息叛乱的道路。游戏共有两种职业可由玩家选择，分别是忍者和武士，忍者特点出其不意，怀中袖刀，苦无飞刀不在话下；武士擅长坚忍反击，格挡来犯的攻击，后发制人。游戏中的敌人会在一定区域内巡逻，因此游戏添加了隐藏元素，玩家如果多观察敌人走向，并找到合适的时机躲入黑暗，可以避免一场原本致命的冲突。

**游戏背景：**游戏的故事背景设定在一个虚构的古代东方国度，类似于日本幕府时代，但不直接对应历史上的任何具体时期。在这个时代里，国家曾经在一位明智而仁慈的将军领导下繁荣昌盛，百姓们享受着和平与安定的生活。然而，一场突如其来的叛乱打破了这片宁静，导致了社会动荡不安。但后来因为叛军的军队管理混乱和内部矛盾问题，势力渐微，将军意识到是时候该派出得力干将了，人民，已经渴望这场胜利很久了。

**项目功能：**

- 使用有限状态机实现玩家角色控制器与AI，且通过动画事件与角色控制状态机联合（例如攻击帧判定，玩家预输入阶段，允许打断动作的时机），实现角色闲置，移动、跳跃、攻击、射箭，投掷，弹反、僵直等状态。
- 使用UI Manager管理面板的加载与卸载，暂停和恢复等行为。
- 使用Shader Graph编写着色器，实现限定只有角色武器或特殊部位产生辉光效果。
- 使用SceneManager管理场景加载后的业务逻辑。
