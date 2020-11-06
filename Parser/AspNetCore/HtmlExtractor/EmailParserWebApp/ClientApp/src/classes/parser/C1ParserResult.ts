export class C1ParserResult {
    public static STATUS_NONE: number = -1;
    public static STATUS_PARSING: number = 0;
    public static STATUS_DONE: number = 1;

    Data: any;

    constructor(data) {
        this.Data = data;
    }

   
}