# One billon rows challenge
```shell
dotnet publish /p:DebugType=None /p:DebugSymbols=false --self-contained false --configuration Release --runtime osx-arm64 -p:PublishSingleFile=true OneBillonRows/OneBillonRows.csproj
```

```shell
time ./artifacts/publish/OneBillonRows/release_osx-arm64/OneBillonRows measurements_100M.txt
```

[Install hyperfine](https://github.com/sharkdp/hyperfine?tab=readme-ov-file#installation)

```shell
dotnet publish /p:DebugType=None /p:DebugSymbols=false --self-contained false --configuration Release --runtime osx-arm64 -p:PublishSingleFile=true OneBillonRows/OneBillonRows.csproj && hyperfine --warmup 3 "./artifacts/publish/OneBillonRows/release_osx-arm64/OneBillonRows measurements_100M.txt" --export-json ./artifacts/publish/OneBillonRows/release_osx-arm64/results.json --export-markdown ./artifacts/publish/OneBillonRows/release_osx-arm64/results.md
```