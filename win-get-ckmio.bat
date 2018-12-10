call rmdir /S /Q .\src\Ckmio
call git clone https://github.com/ckmio/ckmio-csharp _build/
xcopy /s /i _build\Ckmio .\src\Ckmio