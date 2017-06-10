using System.Runtime.Serialization;

namespace Codolith.Serialization {
[DataContract]class ModTuple<T1,T2>{
public ModTuple(T1 v1=default(T1),T2 v2=default(T2)){
Value1 = v1;
Value2 = v2;
}
[DataMember]public T1 Value1{get;set;}
[DataMember]public T2 Value2{get;set;}
}
[DataContract]class ModTuple<T1,T2,T3>{
public ModTuple(T1 v1=default(T1),T2 v2=default(T2),T3 v3=default(T3)){
Value1 = v1;
Value2 = v2;
Value3 = v3;
}
[DataMember]public T1 Value1{get;set;}
[DataMember]public T2 Value2{get;set;}
[DataMember]public T3 Value3{get;set;}
}
[DataContract]class ModTuple<T1,T2,T3,T4>{
public ModTuple(T1 v1=default(T1),T2 v2=default(T2),T3 v3=default(T3),T4 v4=default(T4)){
Value1 = v1;
Value2 = v2;
Value3 = v3;
Value4 = v4;
}
[DataMember]public T1 Value1{get;set;}
[DataMember]public T2 Value2{get;set;}
[DataMember]public T3 Value3{get;set;}
[DataMember]public T4 Value4{get;set;}
}
[DataContract]class ModTuple<T1,T2,T3,T4,T5>{
public ModTuple(T1 v1=default(T1),T2 v2=default(T2),T3 v3=default(T3),T4 v4=default(T4),T5 v5=default(T5)){
Value1 = v1;
Value2 = v2;
Value3 = v3;
Value4 = v4;
Value5 = v5;
}
[DataMember]public T1 Value1{get;set;}
[DataMember]public T2 Value2{get;set;}
[DataMember]public T3 Value3{get;set;}
[DataMember]public T4 Value4{get;set;}
[DataMember]public T5 Value5{get;set;}
}
[DataContract]class ModTuple<T1,T2,T3,T4,T5,T6>{
public ModTuple(T1 v1=default(T1),T2 v2=default(T2),T3 v3=default(T3),T4 v4=default(T4),T5 v5=default(T5),T6 v6=default(T6)){
Value1 = v1;
Value2 = v2;
Value3 = v3;
Value4 = v4;
Value5 = v5;
Value6 = v6;
}
[DataMember]public T1 Value1{get;set;}
[DataMember]public T2 Value2{get;set;}
[DataMember]public T3 Value3{get;set;}
[DataMember]public T4 Value4{get;set;}
[DataMember]public T5 Value5{get;set;}
[DataMember]public T6 Value6{get;set;}
}
[DataContract]class ModTuple<T1,T2,T3,T4,T5,T6,T7>{
public ModTuple(T1 v1=default(T1),T2 v2=default(T2),T3 v3=default(T3),T4 v4=default(T4),T5 v5=default(T5),T6 v6=default(T6),T7 v7=default(T7)){
Value1 = v1;
Value2 = v2;
Value3 = v3;
Value4 = v4;
Value5 = v5;
Value6 = v6;
Value7 = v7;
}
[DataMember]public T1 Value1{get;set;}
[DataMember]public T2 Value2{get;set;}
[DataMember]public T3 Value3{get;set;}
[DataMember]public T4 Value4{get;set;}
[DataMember]public T5 Value5{get;set;}
[DataMember]public T6 Value6{get;set;}
[DataMember]public T7 Value7{get;set;}
}
[DataContract]class ModTuple<T1,T2,T3,T4,T5,T6,T7,T8>{
public ModTuple(T1 v1=default(T1),T2 v2=default(T2),T3 v3=default(T3),T4 v4=default(T4),T5 v5=default(T5),T6 v6=default(T6),T7 v7=default(T7),T8 v8=default(T8)){
Value1 = v1;
Value2 = v2;
Value3 = v3;
Value4 = v4;
Value5 = v5;
Value6 = v6;
Value7 = v7;
Value8 = v8;
}
[DataMember]public T1 Value1{get;set;}
[DataMember]public T2 Value2{get;set;}
[DataMember]public T3 Value3{get;set;}
[DataMember]public T4 Value4{get;set;}
[DataMember]public T5 Value5{get;set;}
[DataMember]public T6 Value6{get;set;}
[DataMember]public T7 Value7{get;set;}
[DataMember]public T8 Value8{get;set;}
}
[DataContract]class ModTuple<T1,T2,T3,T4,T5,T6,T7,T8,T9>{
public ModTuple(T1 v1=default(T1),T2 v2=default(T2),T3 v3=default(T3),T4 v4=default(T4),T5 v5=default(T5),T6 v6=default(T6),T7 v7=default(T7),T8 v8=default(T8),T9 v9=default(T9)){
Value1 = v1;
Value2 = v2;
Value3 = v3;
Value4 = v4;
Value5 = v5;
Value6 = v6;
Value7 = v7;
Value8 = v8;
Value9 = v9;
}
[DataMember]public T1 Value1{get;set;}
[DataMember]public T2 Value2{get;set;}
[DataMember]public T3 Value3{get;set;}
[DataMember]public T4 Value4{get;set;}
[DataMember]public T5 Value5{get;set;}
[DataMember]public T6 Value6{get;set;}
[DataMember]public T7 Value7{get;set;}
[DataMember]public T8 Value8{get;set;}
[DataMember]public T9 Value9{get;set;}
}
[DataContract]class ModTuple<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>{
public ModTuple(T1 v1=default(T1),T2 v2=default(T2),T3 v3=default(T3),T4 v4=default(T4),T5 v5=default(T5),T6 v6=default(T6),T7 v7=default(T7),T8 v8=default(T8),T9 v9=default(T9),T10 v10=default(T10)){
Value1 = v1;
Value2 = v2;
Value3 = v3;
Value4 = v4;
Value5 = v5;
Value6 = v6;
Value7 = v7;
Value8 = v8;
Value9 = v9;
Value10 = v10;
}
[DataMember]public T1 Value1{get;set;}
[DataMember]public T2 Value2{get;set;}
[DataMember]public T3 Value3{get;set;}
[DataMember]public T4 Value4{get;set;}
[DataMember]public T5 Value5{get;set;}
[DataMember]public T6 Value6{get;set;}
[DataMember]public T7 Value7{get;set;}
[DataMember]public T8 Value8{get;set;}
[DataMember]public T9 Value9{get;set;}
[DataMember]public T10 Value10{get;set;}
}
}

