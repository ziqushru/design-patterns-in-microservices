import type { Provider } from "@/models/Provider";

export function useProviders() {

    const http = useHttp();

    const getById = async (id: string) => {
        return await http.get<Provider>('/providers/get-by-id/v1?id=' + id);
    }

    const getAll = async () => {
        return await http.get<Array<Provider>>('/providers/get-all/v1');
    }

    const create = async (provider: Provider) => {
        return await http.post<Provider>('/providers/create/v1', provider);
    }

    const update = async (provider: Provider) => {
        return await http.put<Provider>('/providers/update/v1', provider);
    }

    const remove = async (id: string) => {
        return await http.remove<Provider>('/providers/delete/v1', { id: id });
    }

    return {
        getById,
        getAll,
        create,
        update,
        remove
    }
}
