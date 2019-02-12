import { getHrefWithThrow } from './helpers';
import { TransactionResponse } from "tardis-bank-dtos";
import { Transaction } from '../../model';

export const fromTransactionResponseToTransaction = (dto: TransactionResponse) : Transaction => {
    const getHrefs = getHrefWithThrow(dto.Links);
    const transactionId = getHrefs('self');
    return {
        id: transactionId,
        amount: dto.Amount,
        balance: dto.Balance,
        date: new Date(dto.TransactionDate),
        operations: {
            self: transactionId
        }
    }
}
