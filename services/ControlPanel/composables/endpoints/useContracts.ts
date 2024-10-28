import type { Contract } from "@/models/Contract";

export function useContracts() {

    const http = useHttp();

    const getById = async (id: string) => {
        return await http.get<Contract>('/contracts/get-by-id/v1?id=' + id);
    }

    const getAll = async () => {
        return await http.get<Array<Contract>>('/contracts/get-all/v1');
    }

    const create = async (contract: Contract) => {
        return await http.post<null>('/contracts/create/v1', contract);
    }

    const update = async (contract: Contract) => {
        return await http.put<null>('/contracts/update/v1', contract);
    }

    const remove = async (id: string) => {
        return await http.remove<null>('/contracts/delete/v1', { id: id });
    }

    return {
        getById,
        getAll,
        create,
        update,
        remove
    }
}
