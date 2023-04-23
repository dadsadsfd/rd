//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a RdGen v1.11.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#include "OpenStruct_Unknown.Generated.h"



#ifdef _MSC_VER
#pragma warning( push )
#pragma warning( disable:4250 )
#pragma warning( disable:4307 )
#pragma warning( disable:4267 )
#pragma warning( disable:4244 )
#pragma warning( disable:4100 )
#endif

namespace org.example {
// companion
// constants
// initializer
void OpenStruct_Unknown::initialize()
{
}
// primary ctor
OpenStruct_Unknown::OpenStruct_Unknown(int32_t baseField_, rd::RdId unknownId_, rd::Buffer::ByteArray unknownBytes_) :
OpenStruct(std::move(baseField_)), rd::IUnknownInstance(std::move(unknownId_))

{
    initialize();
}
// secondary constructor
// default ctors and dtors
// reader
OpenStruct_Unknown OpenStruct_Unknown::read(rd::SerializationCtx& ctx, rd::Buffer & buffer)
{
    throw std::logic_error("Unknown instances should not be read via serializer");
}
// writer
void OpenStruct_Unknown::write(rd::SerializationCtx& ctx, rd::Buffer& buffer) const
{
    buffer.write_integral(baseField_);
    buffer.write_byte_array_raw(unknownBytes_);
}
// virtual init
// identify
// getters
// intern
// equals trait
bool OpenStruct_Unknown::equals(rd::ISerializable const& object) const
{
    auto const &other = dynamic_cast<OpenStruct_Unknown const&>(object);
    if (this == &other) return true;
    if (this->baseField_ != other.baseField_) return false;
    
    return true;
}
// equality operators
bool operator==(const OpenStruct_Unknown &lhs, const OpenStruct_Unknown &rhs) {
    if (lhs.type_name() != rhs.type_name()) return false;
    return lhs.equals(rhs);
}
bool operator!=(const OpenStruct_Unknown &lhs, const OpenStruct_Unknown &rhs){
    return !(lhs == rhs);
}
// hash code trait
size_t OpenStruct_Unknown::hashCode() const noexcept
{
    size_t __r = 0;
    __r = __r * 31 + (rd::hash<int32_t>()(get_baseField()));
    return __r;
}
// type name trait
std::string OpenStruct_Unknown::type_name() const
{
    return "OpenStruct_Unknown";
}
// static type name trait
std::string OpenStruct_Unknown::static_type_name()
{
    return "OpenStruct_Unknown";
}
// polymorphic to string
std::string OpenStruct_Unknown::toString() const
{
    std::string res = "OpenStruct_Unknown\n";
    res += "\tbaseField = ";
    res += rd::to_string(baseField_);
    res += '\n';
    return res;
}
// external to string
std::string to_string(const OpenStruct_Unknown & value)
{
    return value.toString();
}
}

#ifdef _MSC_VER
#pragma warning( pop )
#endif
