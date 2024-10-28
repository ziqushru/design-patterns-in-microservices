import type { Consumer } from "@/models/Consumer";

export function useConsumers() {

    const http = useHttp();

    const getById = async (id: string) => {
        return await http.get<Consumer>(`/consumers/get-by-id/v1?id=${id}`);
    }

    const getAll = async () => {
        return await http.get<Array<Consumer>>('/consumers/get-all/v1');
    }

    const create = async (consumer: Consumer) => {
        return await http.post<null>('/consumers/create/v1', consumer);
    }

    const update = async (consumer: Consumer) => {
        return await http.put<null>('/consumers/update/v1', consumer);
    }

    const remove = async (id: string) => {
        return await http.remove<null>('/consumers/delete/v1', { id: id });
    }

    return {
        getById,
        getAll,
        create,
        update,
        remove
    }
}
