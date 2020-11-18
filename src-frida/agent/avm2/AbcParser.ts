import { PoolObject } from "./PoolObject";
import { MethodInfo } from "./MethodInfo";

export class AbcParser {

    addr: NativePointer;
    pool: PoolObject;

    constructor (addr: NativePointer) {
        this.addr = addr;
        this.pool = new PoolObject(addr.add(56).readPointer());
    }

    getMethodInfo(index: number): MethodInfo {
        let count = this.pool.methodCount();
        if (index >= count) {
            throw Error('index >= count for getMethodInfo.');
        }

        let result = this.pool.getMethodInfo(index);
        if (result.isNull()) {
            throw Error('got null for getMethodInfo.');
        }

        return new MethodInfo(result);
    }

}