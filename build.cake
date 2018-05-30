#addin "wk.StartProcess"

using PS  = StartProcess.Processor;

Task("Start-Postgres").Does(() => {
    PS.StartProcess("docker-compose down");
    PS.StartProcess("docker-compose up");
});

var target = Argument("target", "Start-Postgres");
RunTarget(target);