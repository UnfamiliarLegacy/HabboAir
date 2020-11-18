// methodInfo + 0  = base + 0x18E6970 (vtable)
// methodInfo + 48 = * pool
// methodInfo + 56 = * _abc_info_pos
// methodInfo + 64 = int method_id
// methodInfo + 72 = _native.thunker 
// methodInfo + 80 = _native.handler (AvmThunkNativeHandler)

export class MethodInfo {

    addr: NativePointer;

    constructor(addr: NativePointer) {
        this.addr = addr;
    }

    public methodId(): number {
        return this.addr.add(64).readInt();
    }

    public nativeThunker(): NativePointer {
        return this.addr.add(72).readPointer();
    }

    public nativeHandler(): NativePointer {
        return this.addr.add(80).readPointer();
    }

}