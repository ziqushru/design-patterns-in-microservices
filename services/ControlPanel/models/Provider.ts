import type { Entity } from "./Entity";

export type Provider = Entity & {
    brandName: string;
    vat: string;
    email: string;
}
