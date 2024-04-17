# One billon rows challenge
```shell
dotnet publish  /p:DebugType=None /p:DebugSymbols=false --self-contained true --configuration Release --runtime osx-arm64 -p:PublishSingleFile=true OneBillonRows/OneBillonRows.csproj
```

```shell
./artifacts/publish/OneBillonRows/release_osx-arm64/OneBillonRows measurements.txt
```

[Install hyperfine](https://github.com/sharkdp/hyperfine?tab=readme-ov-file#installation)

```shell
hyperfine --warmup 3 "./artifacts/publish/OneBillonRows/release_osx-arm64/OneBillonRows measurements.txt" --export-json ./artifacts/publish/OneBillonRows/release_osx-arm64/results.json --export-markdown ./artifacts/publish/OneBillonRows/release_osx-arm64/results.md
```