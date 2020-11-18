import { baseAddress } from "../global";
import { GCList } from "./GCList";
import { DataList } from "./DataList";
import { Stringp } from "./Stringp";

// pool + 376  = GCList<MethodInfo> GC_STRUCTURE(_methods)
// pool + 416  = DataList<int32_t>  GC_STRUCTURE(_method_name_indices);

const PoolObject_GetMethodInfoName = new NativeFunction(
    baseAddress.add('0x507D74'),
    'pointer',
    ['pointer', 'uint']
);

export class PoolObject {

    _addr: NativePointer;
    _methods: GCList;
    _method_name_indices: DataList;

    constructor (addr: NativePointer) {
        this._addr = addr;
        this._methods = new GCList(addr.add(376));
        this._method_name_indices = new DataList(addr.add(416), 'int32');
    }

    methodCount(): number {
        return this._methods.len();
    }

    getMethodInfo(index: number): NativePointer {
        return this._methods.get(index);
    }

    getMethodInfoName(index: number): Stringp | null {
        let ptr = PoolObject_GetMethodInfoName(this._addr, index) as NativePointer;
        if (ptr.isNull()) {
            return null;
        }

        return new Stringp(ptr);
    }

}