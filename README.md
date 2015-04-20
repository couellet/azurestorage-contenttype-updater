This console application is to update Azure Blob Storage files content type. By default a file uploaded will have `application/octet-stream` as content type. This can cause some issues when you need these files via a CDN or something like this for a web application.

## Usage

Basic use case, all files in every containers will be updated.

```
AzureStorageContentTypeUpdater.exe --account ACCOUNT_NAME --key KEY
```

If you need to update files from a particulair container, you can use the `--container` switch.

```
AzureStorageContentTypeUpdater.exe --account ACCOUNT_NAME --key KEY --container public
```

## Switches

`a` or `account`

This is the switch to specify with azure blob storage account you want to use.

`-a ACCOUNT_NAME` or `--account ACCOUNT_NAME`

---

`k` or `key`

This is the switch to specify the access key for your azure blob storage account.

`-k ACCESS_KEY` or `--key ACCESS_KEY`

---

`c` or `container`

This is an optional switch if you need to specify a particular container in the azure blob storage account

`-c CONTAINER` or `--container CONTAINER`

---

`lowercase`

This optional switch can be used to make sure blob paths are in lower case. File names will be untouched by the paths will be in lowercase. For example, `Scripts/Test.js` will become `scripts/Test.js`.

`--lowercase true`