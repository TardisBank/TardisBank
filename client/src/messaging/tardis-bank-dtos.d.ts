declare module "tardis-bank-dtos" {
    interface LinkDto {
        Rel: string, 
        Href: string
    }

    interface BaseDto {
        Links: Array<LinkDto>,
    }

    interface LoginResultDto extends BaseDto{
        Token: string    
    }

    interface HomeResultDto extends BaseDto {
        Email: string;
    }
}