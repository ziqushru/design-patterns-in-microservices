import type { Entity } from "./Entity";

export type Consumer = Entity & {
    firstName: string;
    lastName: string;
    email: string;
    cellPhone: string;
    type: ConsumerType;
}

export enum ConsumerType {

    NotDefined = 1,
    Individual = 2,
    Company = 2
}

export const ConsumerTypeOptions = [
    { value: ConsumerType.NotDefined, label: "Μη Ορισμένο" },
    { value: ConsumerType.Individual, label: "Φυσικό Πρόσωπο" },
    { value: ConsumerType.Company, label: "Επιχείρηση" }
];
