export type ResponseModel<T> =
    {
        success: true;
        data: T;
    } |
    {
        success: false;
        message: string;
    };
