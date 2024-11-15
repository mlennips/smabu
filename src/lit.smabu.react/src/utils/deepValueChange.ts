
export const deepValueChange = (data: any, name: string, value: any) : any => {
    if (name.split(".").length == 1) {
        return { ...data, [name]: value };
    } else {
        var result = { ...data };
        deepSet(result, name, value);
        return result ;
    }
}

const deepSet = (obj: any, path: string, val: any) => {
    const keys = path.split(".");
    for (let i = 0; i < keys.length; i++) {
        let currentKey = keys[i] as any;
        let nextKey = keys[i + 1] as any;
        if (currentKey.includes("[")) {
            currentKey = parseInt(currentKey.substring(1, currentKey.length - 1));
        }
        if (nextKey && nextKey.includes("[")) {
            nextKey = parseInt(nextKey.substring(1, nextKey.length - 1));
        }

        if (currentKey === "__proto__" || currentKey === "constructor") continue;
        if (typeof nextKey !== "undefined") {
            obj[currentKey] = obj[currentKey] ? obj[currentKey] : (isNaN(nextKey) ? {} : []);
        } else {
            obj[currentKey] = val;
        }

        obj = obj[currentKey];
    }
};