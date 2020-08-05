#if BURST_INTERNAL || UNITY_BURST_EXPERIMENTAL_NEON_INTRINSICS
using System;
using System.Diagnostics;

namespace Unity.Burst.Intrinsics
{
    public unsafe static partial class Arm
    {
        /// <summary>
        /// Neon intrinsics
        /// </summary>
        public unsafe partial class Neon
        {
            /// <summary>Insert vector element from another vector element. This instruction copies the vector element of the source SIMD&amp;FP register to the specified vector element of the destination SIMD&amp;FP register.This instruction can insert data into individual elements within a SIMD&amp;FP register without clearing the remaining bits to zero.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>INS Vd.D[0],Xn</c></summary>
            [DebuggerStepThrough]
            public static v64 vcreate_s8(UInt64 a0)
            {
                return new v64(a0);
            }

            /// <summary>Insert vector element from another vector element. This instruction copies the vector element of the source SIMD&amp;FP register to the specified vector element of the destination SIMD&amp;FP register.This instruction can insert data into individual elements within a SIMD&amp;FP register without clearing the remaining bits to zero.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>INS Vd.D[0],Xn</c></summary>
            [DebuggerStepThrough]
            public static v64 vcreate_s16(UInt64 a0)
            {
                return new v64(a0);
            }

            /// <summary>Insert vector element from another vector element. This instruction copies the vector element of the source SIMD&amp;FP register to the specified vector element of the destination SIMD&amp;FP register.This instruction can insert data into individual elements within a SIMD&amp;FP register without clearing the remaining bits to zero.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>INS Vd.D[0],Xn</c></summary>
            [DebuggerStepThrough]
            public static v64 vcreate_s32(UInt64 a0)
            {
                return new v64(a0);
            }

            /// <summary>Insert vector element from another vector element. This instruction copies the vector element of the source SIMD&amp;FP register to the specified vector element of the destination SIMD&amp;FP register.This instruction can insert data into individual elements within a SIMD&amp;FP register without clearing the remaining bits to zero.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>INS Vd.D[0],Xn</c></summary>
            [DebuggerStepThrough]
            public static v64 vcreate_s64(UInt64 a0)
            {
                return new v64(a0);
            }

            /// <summary>Insert vector element from another vector element. This instruction copies the vector element of the source SIMD&amp;FP register to the specified vector element of the destination SIMD&amp;FP register.This instruction can insert data into individual elements within a SIMD&amp;FP register without clearing the remaining bits to zero.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>INS Vd.D[0],Xn</c></summary>
            [DebuggerStepThrough]
            public static v64 vcreate_u8(UInt64 a0)
            {
                return new v64(a0);
            }

            /// <summary>Insert vector element from another vector element. This instruction copies the vector element of the source SIMD&amp;FP register to the specified vector element of the destination SIMD&amp;FP register.This instruction can insert data into individual elements within a SIMD&amp;FP register without clearing the remaining bits to zero.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>INS Vd.D[0],Xn</c></summary>
            [DebuggerStepThrough]
            public static v64 vcreate_u16(UInt64 a0)
            {
                return new v64(a0);
            }

            /// <summary>Insert vector element from another vector element. This instruction copies the vector element of the source SIMD&amp;FP register to the specified vector element of the destination SIMD&amp;FP register.This instruction can insert data into individual elements within a SIMD&amp;FP register without clearing the remaining bits to zero.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>INS Vd.D[0],Xn</c></summary>
            [DebuggerStepThrough]
            public static v64 vcreate_u32(UInt64 a0)
            {
                return new v64(a0);
            }

            /// <summary>Insert vector element from another vector element. This instruction copies the vector element of the source SIMD&amp;FP register to the specified vector element of the destination SIMD&amp;FP register.This instruction can insert data into individual elements within a SIMD&amp;FP register without clearing the remaining bits to zero.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>INS Vd.D[0],Xn</c></summary>
            [DebuggerStepThrough]
            public static v64 vcreate_u64(UInt64 a0)
            {
                return new v64(a0);
            }

            /// <summary>Insert vector element from another vector element. This instruction copies the vector element of the source SIMD&amp;FP register to the specified vector element of the destination SIMD&amp;FP register.This instruction can insert data into individual elements within a SIMD&amp;FP register without clearing the remaining bits to zero.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>INS Vd.D[0],Xn</c></summary>
            [DebuggerStepThrough]
            public static v64 vcreate_f16(UInt64 a0)
            {
                return new v64(a0);
            }

            /// <summary>Insert vector element from another vector element. This instruction copies the vector element of the source SIMD&amp;FP register to the specified vector element of the destination SIMD&amp;FP register.This instruction can insert data into individual elements within a SIMD&amp;FP register without clearing the remaining bits to zero.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>INS Vd.D[0],Xn</c></summary>
            [DebuggerStepThrough]
            public static v64 vcreate_f32(UInt64 a0)
            {
                return new v64(a0);
            }

            /// <summary>Insert vector element from another vector element. This instruction copies the vector element of the source SIMD&amp;FP register to the specified vector element of the destination SIMD&amp;FP register.This instruction can insert data into individual elements within a SIMD&amp;FP register without clearing the remaining bits to zero.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>INS Vd.D[0],Xn</c></summary>
            [DebuggerStepThrough]
            public static v64 vcreate_f64(UInt64 a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.8B,rn</c></summary>
            [DebuggerStepThrough]
            public static v64 vdup_n_s8(SByte a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.16B,rn</c></summary>
            [DebuggerStepThrough]
            public static v128 vdupq_n_s8(SByte a0)
            {
                return new v128(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.4H,rn</c></summary>
            [DebuggerStepThrough]
            public static v64 vdup_n_s16(Int16 a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.8H,rn</c></summary>
            [DebuggerStepThrough]
            public static v128 vdupq_n_s16(Int16 a0)
            {
                return new v128(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.2S,rn</c></summary>
            [DebuggerStepThrough]
            public static v64 vdup_n_s32(Int32 a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.4S,rn</c></summary>
            [DebuggerStepThrough]
            public static v128 vdupq_n_s32(Int32 a0)
            {
                return new v128(a0);
            }

            /// <summary>Insert vector element from another vector element. This instruction copies the vector element of the source SIMD&amp;FP register to the specified vector element of the destination SIMD&amp;FP register.This instruction can insert data into individual elements within a SIMD&amp;FP register without clearing the remaining bits to zero.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>INS Dd.D[0],xn</c></summary>
            [DebuggerStepThrough]
            public static v64 vdup_n_s64(Int64 a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.2D,rn</c></summary>
            [DebuggerStepThrough]
            public static v128 vdupq_n_s64(Int64 a0)
            {
                return new v128(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.8B,rn</c></summary>
            [DebuggerStepThrough]
            public static v64 vdup_n_u8(Byte a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.16B,rn</c></summary>
            [DebuggerStepThrough]
            public static v128 vdupq_n_u8(Byte a0)
            {
                return new v128(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.4H,rn</c></summary>
            [DebuggerStepThrough]
            public static v64 vdup_n_u16(UInt16 a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.8H,rn</c></summary>
            [DebuggerStepThrough]
            public static v128 vdupq_n_u16(UInt16 a0)
            {
                return new v128(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.2S,rn</c></summary>
            [DebuggerStepThrough]
            public static v64 vdup_n_u32(UInt32 a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.4S,rn</c></summary>
            [DebuggerStepThrough]
            public static v128 vdupq_n_u32(UInt32 a0)
            {
                return new v128(a0);
            }

            /// <summary>Insert vector element from another vector element. This instruction copies the vector element of the source SIMD&amp;FP register to the specified vector element of the destination SIMD&amp;FP register.This instruction can insert data into individual elements within a SIMD&amp;FP register without clearing the remaining bits to zero.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>INS Dd.D[0],xn</c></summary>
            [DebuggerStepThrough]
            public static v64 vdup_n_u64(UInt64 a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.2D,rn</c></summary>
            [DebuggerStepThrough]
            public static v128 vdupq_n_u64(UInt64 a0)
            {
                return new v128(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.2S,rn</c></summary>
            [DebuggerStepThrough]
            public static v64 vdup_n_f32(Single a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.4S,rn</c></summary>
            [DebuggerStepThrough]
            public static v128 vdupq_n_f32(Single a0)
            {
                return new v128(a0);
            }

            /// <summary>Insert vector element from another vector element. This instruction copies the vector element of the source SIMD&amp;FP register to the specified vector element of the destination SIMD&amp;FP register.This instruction can insert data into individual elements within a SIMD&amp;FP register without clearing the remaining bits to zero.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>INS Dd.D[0],xn</c></summary>
            [DebuggerStepThrough]
            public static v64 vdup_n_f64(Double a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.2D,rn</c></summary>
            [DebuggerStepThrough]
            public static v128 vdupq_n_f64(Double a0)
            {
                return new v128(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.8B,rn</c></summary>
            [DebuggerStepThrough]
            public static v64 vmov_n_s8(SByte a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.16B,rn</c></summary>
            [DebuggerStepThrough]
            public static v128 vmovq_n_s8(SByte a0)
            {
                return new v128(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.4H,rn</c></summary>
            [DebuggerStepThrough]
            public static v64 vmov_n_s16(Int16 a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.8H,rn</c></summary>
            [DebuggerStepThrough]
            public static v128 vmovq_n_s16(Int16 a0)
            {
                return new v128(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.2S,rn</c></summary>
            [DebuggerStepThrough]
            public static v64 vmov_n_s32(Int32 a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.4S,rn</c></summary>
            [DebuggerStepThrough]
            public static v128 vmovq_n_s32(Int32 a0)
            {
                return new v128(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,rn</c></summary>
            [DebuggerStepThrough]
            public static v64 vmov_n_s64(Int64 a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.2D,rn</c></summary>
            [DebuggerStepThrough]
            public static v128 vmovq_n_s64(Int64 a0)
            {
                return new v128(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.8B,rn</c></summary>
            [DebuggerStepThrough]
            public static v64 vmov_n_u8(Byte a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.16B,rn</c></summary>
            [DebuggerStepThrough]
            public static v128 vmovq_n_u8(Byte a0)
            {
                return new v128(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.4H,rn</c></summary>
            [DebuggerStepThrough]
            public static v64 vmov_n_u16(UInt16 a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.8H,rn</c></summary>
            [DebuggerStepThrough]
            public static v128 vmovq_n_u16(UInt16 a0)
            {
                return new v128(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.2S,rn</c></summary>
            [DebuggerStepThrough]
            public static v64 vmov_n_u32(UInt32 a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.4S,rn</c></summary>
            [DebuggerStepThrough]
            public static v128 vmovq_n_u32(UInt32 a0)
            {
                return new v128(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,rn</c></summary>
            [DebuggerStepThrough]
            public static v64 vmov_n_u64(UInt64 a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.2D,rn</c></summary>
            [DebuggerStepThrough]
            public static v128 vmovq_n_u64(UInt64 a0)
            {
                return new v128(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.2S,rn</c></summary>
            [DebuggerStepThrough]
            public static v64 vmov_n_f32(Single a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.4S,rn</c></summary>
            [DebuggerStepThrough]
            public static v128 vmovq_n_f32(Single a0)
            {
                return new v128(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,rn</c></summary>
            [DebuggerStepThrough]
            public static v64 vmov_n_f64(Double a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.2D,rn</c></summary>
            [DebuggerStepThrough]
            public static v128 vmovq_n_f64(Double a0)
            {
                return new v128(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[0]</c></summary>
            [DebuggerStepThrough]
            public static v128 vcombine_s8(v64 a0, v64 a1)
            {
                return new v128(a0, a1);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[0]</c></summary>
            [DebuggerStepThrough]
            public static v128 vcombine_s16(v64 a0, v64 a1)
            {
                return new v128(a0, a1);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[0]</c></summary>
            [DebuggerStepThrough]
            public static v128 vcombine_s32(v64 a0, v64 a1)
            {
                return new v128(a0, a1);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[0]</c></summary>
            [DebuggerStepThrough]
            public static v128 vcombine_s64(v64 a0, v64 a1)
            {
                return new v128(a0, a1);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[0]</c></summary>
            [DebuggerStepThrough]
            public static v128 vcombine_u8(v64 a0, v64 a1)
            {
                return new v128(a0, a1);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[0]</c></summary>
            [DebuggerStepThrough]
            public static v128 vcombine_u16(v64 a0, v64 a1)
            {
                return new v128(a0, a1);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[0]</c></summary>
            [DebuggerStepThrough]
            public static v128 vcombine_u32(v64 a0, v64 a1)
            {
                return new v128(a0, a1);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[0]</c></summary>
            [DebuggerStepThrough]
            public static v128 vcombine_u64(v64 a0, v64 a1)
            {
                return new v128(a0, a1);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[0]</c></summary>
            [DebuggerStepThrough]
            public static v128 vcombine_f16(v64 a0, v64 a1)
            {
                return new v128(a0, a1);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[0]</c></summary>
            [DebuggerStepThrough]
            public static v128 vcombine_f32(v64 a0, v64 a1)
            {
                return new v128(a0, a1);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[0]</c></summary>
            [DebuggerStepThrough]
            public static v128 vcombine_f64(v64 a0, v64 a1)
            {
                return new v128(a0, a1);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[1]</c></summary>
            [DebuggerStepThrough]
            public static v64 vget_high_s8(v128 a0)
            {
                return a0.Hi64;
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[1]</c></summary>
            [DebuggerStepThrough]
            public static v64 vget_high_s16(v128 a0)
            {
                return a0.Hi64;
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[1]</c></summary>
            [DebuggerStepThrough]
            public static v64 vget_high_s32(v128 a0)
            {
                return a0.Hi64;
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[1]</c></summary>
            [DebuggerStepThrough]
            public static v64 vget_high_s64(v128 a0)
            {
                return a0.Hi64;
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[1]</c></summary>
            [DebuggerStepThrough]
            public static v64 vget_high_u8(v128 a0)
            {
                return a0.Hi64;
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[1]</c></summary>
            [DebuggerStepThrough]
            public static v64 vget_high_u16(v128 a0)
            {
                return a0.Hi64;
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[1]</c></summary>
            [DebuggerStepThrough]
            public static v64 vget_high_u32(v128 a0)
            {
                return a0.Hi64;
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[1]</c></summary>
            [DebuggerStepThrough]
            public static v64 vget_high_u64(v128 a0)
            {
                return a0.Hi64;
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[1]</c></summary>
            [DebuggerStepThrough]
            public static v64 vget_high_f16(v128 a0)
            {
                return a0.Hi64;
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[1]</c></summary>
            [DebuggerStepThrough]
            public static v64 vget_high_f32(v128 a0)
            {
                return a0.Hi64;
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[1]</c></summary>
            [DebuggerStepThrough]
            public static v64 vget_high_f64(v128 a0)
            {
                return a0.Hi64;
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[0]</c></summary>
            [DebuggerStepThrough]
            public static v64 vget_low_s8(v128 a0)
            {
                return a0.Lo64;
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[0]</c></summary>
            [DebuggerStepThrough]
            public static v64 vget_low_s16(v128 a0)
            {
                return a0.Lo64;
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[0]</c></summary>
            [DebuggerStepThrough]
            public static v64 vget_low_s32(v128 a0)
            {
                return a0.Lo64;
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[0]</c></summary>
            [DebuggerStepThrough]
            public static v64 vget_low_s64(v128 a0)
            {
                return a0.Lo64;
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[0]</c></summary>
            [DebuggerStepThrough]
            public static v64 vget_low_u8(v128 a0)
            {
                return a0.Lo64;
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[0]</c></summary>
            [DebuggerStepThrough]
            public static v64 vget_low_u16(v128 a0)
            {
                return a0.Lo64;
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[0]</c></summary>
            [DebuggerStepThrough]
            public static v64 vget_low_u32(v128 a0)
            {
                return a0.Lo64;
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[0]</c></summary>
            [DebuggerStepThrough]
            public static v64 vget_low_u64(v128 a0)
            {
                return a0.Lo64;
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[0]</c></summary>
            [DebuggerStepThrough]
            public static v64 vget_low_f16(v128 a0)
            {
                return a0.Lo64;
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[0]</c></summary>
            [DebuggerStepThrough]
            public static v64 vget_low_f32(v128 a0)
            {
                return a0.Lo64;
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.1D,Vn.D[0]</c></summary>
            [DebuggerStepThrough]
            public static v64 vget_low_f64(v128 a0)
            {
                return a0.Lo64;
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.4H,rn</c></summary>
            [DebuggerStepThrough]
            public static v64 vmov_n_f16(Int16 a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.8H,rn</c></summary>
            [DebuggerStepThrough]
            public static v128 vmovq_n_f16(Int16 a0)
            {
                return new v128(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.4H,rn</c></summary>
            [DebuggerStepThrough]
            public static v64 vdup_n_f16(Int16 a0)
            {
                return new v64(a0);
            }

            /// <summary>Duplicate vector element to vector or scalar. This instruction duplicates the vector element at the specified element index in the source SIMD&amp;FP register into a scalar or each element in a vector, and writes the result to the destination SIMD&amp;FP register.Depending on the settings in the CPACR_EL1, CPTR_EL2, and CPTR_EL3 registers, and the current Security state and Exception level, an attempt to execute the instruction might be trapped.
            /// <br/>Equivalent instruction: <c>DUP Vd.8H,rn</c></summary>
            [DebuggerStepThrough]
            public static v128 vdupq_n_f16(Int16 a0)
            {
                return new v128(a0);
            }
        }
    }
}
#endif // BURST_INTERNAL || UNITY_BURST_EXPERIMENTAL_NEON_INTRINSICS
