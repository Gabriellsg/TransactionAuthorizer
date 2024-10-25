CREATE TABLE TRANSACTION_LOG (
    ID INT PRIMARY KEY IDENTITY,
    ACCOUNTID INT NOT NULL,
    AMOUNT DECIMAL(18, 2) NOT NULL,
    MERCHANT NVARCHAR(255) NOT NULL,
    MCCCODE NVARCHAR(4) NOT NULL,
    TRANSACTIONDATE DATETIME NOT NULL DEFAULT GETDATE(),
    AUTHORIZATIONCODE NVARCHAR(2) NOT NULL,
    FOREIGN KEY (ACCOUNTID) REFERENCES ACCOUNT(ID),
    FOREIGN KEY (MCCCODE) REFERENCES MERCHANT_CATEGORY_CODE(MCCCODE)
);