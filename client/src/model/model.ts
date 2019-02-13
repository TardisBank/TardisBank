export type Operations = {
    self: string
}

export type AccountOperations = {
    accountId: string,
    transactions: string,
    schedule: string
} & Operations;

export type Account = {
    id: string,
    name: string,
    operations: AccountOperations
}

export type Accounts = ReadonlyArray<Account>;

export type TransactionOperations = Operations;

export type Transaction = {
    id: string,
    date: Date,
    amount: number,
    balance: number,
    operations: TransactionOperations
}

export type Transactions = ReadonlyArray<Transaction>;