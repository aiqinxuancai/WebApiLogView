# WebApiLogView

用于将其他端的日志通过http方式丢到PC端查看（内网）。

启动后会在顶部显示地址及端口，只需向端口post数据即可。

```json
{"funcName" : "SendLog", "level" : 1, "message" : "logContent~~"}
```

```csharp
public string LevelString { get {
var str = Level switch
{
    1 => "DEBUG",
    2 => "INFO",
    3 => "WARN",
    4 => "ERROR",
    5 => "FATAL",
    6 => "NONE",
    _ => ""
};
return str;
} }

```
