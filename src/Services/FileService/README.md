# 📁 Musdis.FileService

A service for managing file uploading and storing.

## ☁️ Storage

Built with [`Google Firebase Storage`](https://firebase.google.com/docs/storage).

# Quick start

- Create project in [Google Firebase](https://console.firebase.google.com/).

- Add user secrets to your project. [See docs for more information](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets).

- Set following values to your secrets.

```json
{
  "Firebase:ProjectId": "your-project-id",
  "Firebase:KeyPath": "../../../secrets/your/path/to/secrets",
  "Firebase:DefaultBucketName": "your-default-bucket-name"
}
```