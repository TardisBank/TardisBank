import { AccountResponse, LinkDto } from 'tardis-bank-dtos';
import { Account } from '../../model';

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

const getHrefWithThrow = (links: ReadonlyArray<LinkDto>) => (rel: string) => {
    const value = getHrefForRel(links, rel);
    if (!value) {
        throw new Error(`Can not find ${rel} in links`);
    }
    return value;
}

const getHrefForRel = (links: ReadonlyArray<LinkDto>, rel: string): string | undefined => {
    const linkDto = links.find(x => x.Rel === rel);
    return linkDto ? linkDto.Href : undefined;
}