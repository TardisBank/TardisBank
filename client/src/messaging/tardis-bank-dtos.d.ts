declare module "tardis-bank-dtos" {
    interface LinkDto {
        Rel: string, 
        Href: string
    }

    interface LoginResultDto {
        Links: Array<LinkDto>,
        Token: string    
    }
}