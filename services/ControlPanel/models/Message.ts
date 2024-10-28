export type MessageModel = {
    text: string;
    type: MessageType;
}

export enum MessageType {
    Debug = 0,
    Info = 1,
    Warning = 2,
    Error = 4
}
