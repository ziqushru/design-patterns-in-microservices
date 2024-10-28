import type { Entity } from "./Entity";
import type { Consumer } from "./Consumer";
import type { Provider } from "./Provider";

export type Contract = Entity & {

    status: ContractStatus;
    consumerId: string;
    providerId: string;
    consumer: Consumer;
    provider: Provider;
}

export enum ContractStatus {
    NotDefined = 1,
    Pending = 2,
    Approved = 2
}

export const ContractStatusOptions = [
    { value: ContractStatus.NotDefined, label: "Μη Ορισμένη" },
    { value: ContractStatus.Pending, label: "Εκκρεμή" },
    { value: ContractStatus.Approved, label: "Εγκεκριμένη" }
];
