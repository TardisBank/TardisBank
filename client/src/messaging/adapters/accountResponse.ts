import { AccountResponse } from 'tardis-bank-dtos';
import { Account } from '../../model';
import { getHrefWithThrow } from './helpers';

export const fromAccountResponseToAccount = (dto: AccountResponse): Account  => {
    const getHrefs = getHrefWithThrow(dto.Links);
    const accountId = getHrefs('self');
    return {
        id: accountId,
        name: dto.AccountName,
        operations: {
            accountId: accountId,
            self: accountId,
            transactions: getHrefs('transaction'),
            schedule: getHrefs('schedule')
        }
    };
}