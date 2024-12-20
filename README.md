# Bicycle shop (web API)

## Features

- Login, logout using JWT
- Register account
- Password recovery (send email SMTP)
- Cart
- Order
- Payment (VNPay)
- Product
- Product category
- Product detail: CRUD detail, attach & deattach image
- Product image: upload & delete product images
- User

## Technologies

- ASP.NET Core Web API: Entity Framework, code-first migration, repository pattern
- JWT Authentication
- Cloudinary: upload & delete images
- VNPay: payment
- SQL Server

## Installation

1. Clone the repository

```BASH
git clone https://github.com/nhienau/bicycle-shop-api
```

2. Create a .env file and specify all environment variables (see `api/.env.example`)

3. Run the API in Visual Studio
