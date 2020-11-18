import { AbcParser } from "./avm2/AbcParser";
import { DataList } from "./avm2/DataList";
import { PoolObject } from "./avm2/PoolObject";
import { MethodInfo } from "./avm2/MethodInfo";
import { baseAddress } from "./global";

const debug = false;

// const AbcParser_ResolveMethodInfo = new NativeFunction(baseAddress.add('0x4CB680'), 'pointer', ['pointer', 'pointer']);

// 0x4CB214 AbcParser::Constructor
// 0x4CB130 AbcParser::Parse

// 0x544CEC GCList<MethodInfo>::set 

// 0x4CAB00 AbcParser::decodeAbc(AvmCore* core, ScriptBuffer code, Toplevel* toplevel, Domain* domain, const NativeInitializer* natives, ApiVersion apiVersion)
Interceptor.attach(baseAddress.add('0x4CAB00'), {
    onEnter: function (args) {
        let core = args[0];
        let code = args[1];
        let toplevel = args[2];
        let domain = args[3];
        let natives = args[4];
        let apiVersion = args[5];

        if (natives.isNull()) {
            console.log('[-] AbcParser::decodeAbc(..)');
        } else {
            console.log('[-] AbcParser::decodeAbc(..) for natives');
        }

        this.isNative = !natives.isNull();
    },
    onLeave: function (retval) {
        if (!this.isNative) {
            return;
        }

        let pool = new PoolObject(retval);

        if (debug) {
            console.log('> Found', pool._methods.len(), 'methods.');
            console.log('> Found', pool._method_name_indices.len(), 'method names.');
            
            let methodId = 1000;
            let methodInfoAddr = pool._methods.get(methodId);
            if (methodInfoAddr.isNull()) {
                return;
            }
    
            let methodInfo = new MethodInfo(methodInfoAddr);
    
            console.log('> First method:');
            console.log('name                       ', pool.getMethodInfoName(methodId));
            console.log('name_index                 ', pool._method_name_indices.get(methodInfo.methodId()));
            console.log('method_info->method_id     ', methodInfo.methodId());
            console.log('method_info->native.thunker', methodInfo.nativeThunker().sub(baseAddress));
            console.log('method_info->native.handler', methodInfo.nativeHandler().sub(baseAddress));
        } else {
            console.log('import idaapi');
    
            for (let methodId = 0; methodId < pool.methodCount(); methodId++) {
                let methodInfoAddr = pool._methods.get(methodId);
                if (methodInfoAddr.isNull()) {
                    continue;
                }
    
                let methodInfo = new MethodInfo(methodInfoAddr);
                let methodNameStr = pool.getMethodInfoName(methodId);
                if (methodNameStr === null) {
                    continue;
                }
    
                let methodAddr = methodInfo.nativeHandler().sub(baseAddress).add('0x100000000');
                let methodName = methodNameStr.getString().replace('_-', 'Safe_');
    
                console.log(`idc.MakeName(${methodAddr}, "FLASH_${methodName}")`);
            }
        }
    }
});

// AbcParser::ParseMethodInfos
// Interceptor.attach(baseAddress.add('0x4CCB30'), {
//     onEnter: function (args) {
//         this.self = new AbcParser(args[0]);
//     },
//     onLeave: function (retval) {
//         let self: AbcParser = this.self;

//         console.log('> Found', self.pool.methodCount(), 'methods.');
//         console.log('> Found', self.pool._method_name_indicides.len(), 'method names.');

//         //console.log(hexdump(self.getMethodInfo(0), {length: 64 + 32}));

//         let methodInfo = self.getMethodInfo(39701);

//         // typedef void (AvmPlusScriptableObject::*AvmThunkNativeMethodHandler)();
//         // typedef void (*AvmThunkNativeFunctionHandler)(AvmPlusScriptableObject* obj);

//         console.log('> First method:');
//         console.log('name_index                 ', self.pool._method_name_indicides.get(methodInfo.methodId()));
//         console.log('method_info->method_id     ', methodInfo.methodId());
//         console.log('method_info->native.thunker', methodInfo.nativeThunker().sub(baseAddress));
//         console.log('method_info->native.handler', methodInfo.nativeHandler().sub(baseAddress));
//     }
// });

// Interceptor.attach(baseAddress.add('0x544CEC'), {
//     onEnter: function (args) {
//         console.log(args[0].add(8).readU32());
//     }
// })

// Interceptor.attach(baseAddress.add('0x508660'), {
//     onEnter: function (args) {
//         // let value = args[1].readUtf8String();
//         // if (value == '::' || value == '\n') {
//         //     return;
//         // }
//         // console.log(args[0].readCString());
//         console.log(hexdump(args[1], {length: 64}));
//     }
// });