import axios, { AxiosError } from "axios";
import type { ResponseModel } from "@/models/ResponseModel";

const useHttpService = () => {

    const service = axios.create({
        baseURL: useRuntimeConfig().public.GATEKEEPER_URL
    });

    return service;
}

export function useHttp() {
    const httpService = useHttpService();

    const get = async <T>(path: string): Promise<ResponseModel<T>> => {

        try {
            const response = await httpService.get(path);

            let model = {
                success: true,
                data: response.data
            } as ResponseModel<T>;

            return model;
        } catch (error: any) {

            let model = {
                success: false,
                message: getErrorMessage(error)
            } as ResponseModel<T>;

            return model;
        }
    }

    const post = async <T>(path: string, payload: any): Promise<ResponseModel<T>> => {

        try {
            const response = await httpService.post(path, payload);

            let model = {
                success: true,
                data: response.data
            } as ResponseModel<T>;

            return model;
        } catch (error: any) {

            let model = {
                success: false,
                message: getErrorMessage(error)
            } as ResponseModel<T>;

            return model;
        }
    }

    const put = async <T>(path: string, payload: any): Promise<ResponseModel<T>> => {

        try {
            const response = await httpService.put(path, payload);

            let model = {
                success: true,
                data: response.data
            } as ResponseModel<T>;

            return model;
        } catch (error: any) {

            let model = {
                success: false,
                message: getErrorMessage(error)
            } as ResponseModel<T>;

            return model;
        }
    }

    const patch = async <T>(path: string, payload: any): Promise<ResponseModel<T>> => {

        try {
            const response = await httpService.patch(path, payload);

            let model = {
                success: true,
                data: response.data
            } as ResponseModel<T>;

            return model;
        } catch (error: any) {

            let model = {
                success: false,
                message: getErrorMessage(error)
            } as ResponseModel<T>;

            return model;
        }
    }

    const remove = async <T>(path: string, payload: any): Promise<ResponseModel<T>> => {

        try {
            const response = await httpService.delete(path, { data: payload });

            let model = {
                success: true,
                data: response.data
            } as ResponseModel<T>;

            return model;
        } catch (error: any) {

            let model = {
                success: false,
                message: getErrorMessage(error)
            } as ResponseModel<T>;

            return model;
        }
    }

    const getErrorMessage = (error: AxiosError) => {

        let message = 'Κάτι πήγε στραβά';

        if (!error.response) {
            return message;
        }

        message = (error.response.data as any).detail;

        if (message) {
            return message;
        }

        let status = (error.response?.data as any)?.status ?? 500;

        if (status == 400) {
            message = 'Η ενέργεια απέτυχε.'
        } else if (status == 404) {
            message = 'Η ενέργεια δε βρέθηκε.'
        }

        return message;
    }

    return {
        remove,
        patch,
        post,
        put,
        get
    }
}
