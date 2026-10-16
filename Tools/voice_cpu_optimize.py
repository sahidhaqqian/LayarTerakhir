"""CPU authoring-only optimization; never included in the Unity player.
Keep diffusion/stop/FSQ in FP32; quantize only the autoregressive LMs per channel.
"""
import ctypes,gc,torch

def optimize_model(model):
    torch.backends.quantized.engine='fbgemm'
    names=[n for n,m in model.tts_model.named_modules() if hasattr(m,'qweight') and n.startswith(('base_lm.','residual_lm.','feat_decoder.'))]
    for name in names:
        parent_name,_,key=name.rpartition('.');parent=model.tts_model.get_submodule(parent_name);old=getattr(parent,key)
        weight=old.qweight.float()*old.scale
        linear=torch.nn.Linear(weight.shape[1],weight.shape[0],bias=old.bias is not None)
        linear.weight=torch.nn.Parameter(weight,requires_grad=False)
        if old.bias is not None:linear.bias=torch.nn.Parameter(old.bias.clone(),requires_grad=False)
        if name.startswith(('base_lm.','residual_lm.')):
            linear=torch.ao.quantization.quantize_dynamic(torch.nn.Sequential(linear),{torch.nn.Linear:torch.ao.quantization.per_channel_dynamic_qconfig},inplace=True)[0]
        setattr(parent,key,linear);del old,weight,linear
        gc.collect();ctypes.CDLL('libc.so.6').malloc_trim(0)
    print('CPU optimized: per-channel dynamic AR LMs; full precision diffusion/stop/FSQ',flush=True)
