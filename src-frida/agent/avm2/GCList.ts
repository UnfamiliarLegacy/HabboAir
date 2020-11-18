export class GCList {

    _addr: NativePointer;
    _addr_storage: NativePointer;
    _addr_len: NativePointer;

    constructor (addr: NativePointer) {
        this._addr = addr;
        this._addr_storage = addr.add(0);
        this._addr_len = addr.add(8);
    }

    len(): number {
        return this._addr_len.readU32();
    }

    get(index: number) {
        let count = this.len();
        if (index >= count) {
            return ptr(0);
        }
        
        return this._addr_storage.readPointer().add(8 * index).add(16).readPointer();
    }

}