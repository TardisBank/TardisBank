import { LinkDto } from 'tardis-bank-dtos';

export const getHrefWithThrow = (links: ReadonlyArray<LinkDto>) => (rel: string) => {
    const value = getHrefForRel(links, rel);
    if (!value) {
        throw new Error(`Can not find ${rel} in links`);
    }
    return value;
}

export const getHrefForRel = (links: ReadonlyArray<LinkDto>, rel: string): string | undefined => {
    const linkDto = links.find(x => x.Rel === rel);
    return linkDto ? linkDto.Href : undefined;
}