type DataListType =
    | "int32"
    | "uint32";

export class DataList {

    _addr: NativePointer;
    _type: DataListType;

    constructor(addr: NativePointer, type: DataListType) {
        this._addr = addr;
        this._type = type;
    }

    get(idx: number): number {
        if (this._type == 'int32') {
            // console.log(hexdump(this._addr));
            return this._addr.readPointer().add(4 * idx).add(4).readS32();
        }

        throw Error('Unknown type ' + this._type);
    }

    len(): number {
        return this._addr.add(16).readU32();
    }

}