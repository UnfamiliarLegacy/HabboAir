export class Stringp {

    _addr: NativePointer;
    _m_buffer: NativePointer;
    _m_extra: NativePointer;
    _m_length: number;
    _m_flags: number;

    constructor(addr: NativePointer) {
        this._addr = addr;
        this._m_buffer = addr.add(16).readPointer();
        this._m_extra = addr.add(24).readPointer(); // "Stringp" for dependent strings, "uint32_t index" if not.
        this._m_length = addr.add(32).readS32();
        this._m_flags = addr.add(36).readU32();
    }

    getString(): string {
        return this._m_buffer.readCString(this._m_length)!;
    }

}