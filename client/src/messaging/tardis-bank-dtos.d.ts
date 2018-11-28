declare module "tardis-bank-dtos" {
    interface LinkDto {
        Rel: string, 
        Href: string
    }

    interface BaseDto {
        Links: ReadonlyArray<LinkDto>,
    }

    interface LoginResultDto extends BaseDto{
        Token: string    
    }

    interface HomeResultDto extends BaseDto {
        Email: string;
    }

    interface RegisterRequest {
        Email: string;
        Password: string;
    }

    interface RegisterResponse extends BaseDto {

    }

    interface AccountRequest {
        AccountName: string
    }

    interface AccountResponse extends BaseDto {
        AccountName: string
    }

    interface AccountResponseCollection extends BaseDto {
        Accounts: ReadonlyArray<AccountResponse>
    }
}