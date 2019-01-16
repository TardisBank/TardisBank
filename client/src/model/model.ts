import { Transaction } from './model';
export type Account = {
    id: string,
    name: string,
    operations: AccountOperations
}

export type AccountOperations = {
    accountId: string,
    self: string,
    transactions: string,
    schedule: string
}

export type Accounts = ReadonlyArray<Account>;

export type Transaction = {
    id: string,
    accountId: string,
    date: Date,
    amount: number
}

export type Transactions = ReadonlyArray<Transaction>;