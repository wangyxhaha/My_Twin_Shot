# My_Twin_Shot 工作日志

### 2025-10-1 1:11
以前完全没有Unity基础，不过之前几天学Unity的时候猜测实习可能要做横版跳跃游戏，提前学习了并做了一点FSM，竟然派上用场了

制作的内容：
- 玩家最基础的move和idle状态之间的转移
- move和idle的动画
- 做了惯性滑动

就这么多了，感觉要工期爆炸

### 2025-10-2 1:00

踩坑了

有一个变量`currentAcceleration`用了访问器：
```c#
public Vector2 currentAcceleration { get; private set; }
```
但是后来进行修改的时候忘了，直接调用了`currentAcceleration.Set()`，导致赋值失败

由于`Vector2`是值类型，使用`get`获取的是拷贝，因此`Set()`设置的不是原来的`Vector2`

制作的内容：
- 制作了inAir状态，现在玩家可以正常跳和坠落了
- 修bug

这真做得完吗？不过之后几天时间就多一些了